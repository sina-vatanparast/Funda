using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Funda.DataContract;
using System.Text.Json;
using System.Threading;
using Funda.Exception;
using Funda.ServiceContract;

namespace Funda.Client
{
    public class ObjectApiHttpClient: IObjectApiHttpClient
    {
        // config constansts should be moved out in order to have the same deployment across all environments (in Microservices Architecture)
        private const string Key = "ac1b0b1572524640a0ecc54de453ea9f";
        private const string Api = "https://partnerapi.funda.nl/feeds/Aanbod.svc/json/{0}/?type=koop&zo=/{1}/{2}&page={3}&pagesize={4}";
        private const string Garden = "tuin";
        private const int PageSize = 25;
        private const int RequestLimit = 100;
        private const int Period = 60;
        private const int BackOffRetrySecond = 1;
        private const int MaxRetryCount = 10;

        private readonly HttpClient _httpClient;

        // a single instance of throttler to limit sending Http requests per second across all cuncurrent requests (the first call is excluded)
        private static readonly SemaphoreSlim Throttler = new SemaphoreSlim(RequestLimit - 1);

        public ObjectApiHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        private async Task<FundaApiResponseModel> GetOnePageOfObjects(string city, bool garden, int pageNumber,int pageSize)
        {

            var url = string.Format(Api, Key,  city, garden ? Garden : "", pageNumber, pageSize);
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            // Retry mechanism (in case of unhandled too many requests) based on Maximum retry limit and an incremental delay 
            if (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                var backOffRetrySecond = BackOffRetrySecond;
                var retryCounter = 0;

                
                while (response.StatusCode == HttpStatusCode.TooManyRequests && retryCounter < MaxRetryCount)
                {
                    await Task.Delay(1000 * backOffRetrySecond);
                    response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                    backOffRetrySecond *= 2;
                    retryCounter++;
                }
            }

            // handling the exception of too many requests if delay mechanism is failed
            if (response.StatusCode == HttpStatusCode.TooManyRequests)
                throw new ServerBusyException();

            response.EnsureSuccessStatusCode();

            await using var responseContent = await response.Content.ReadAsStreamAsync();

            return await JsonSerializer.DeserializeAsync<FundaApiResponseModel>(responseContent, DefaultJsonSerializerOptions.Instance);
        }

        public async Task<IReadOnlyCollection<ObjectModel>> Execute(string city, bool garden)
        {
     
            // getting the first page from the server to figrue out all page count in order to create parallel requests
            var firstPage = await GetOnePageOfObjects(city, garden, 1, PageSize);

            if (firstPage.Paging.AantalPaginas == 1) return firstPage.Objects;

            var responses = new List<ObjectModel>();

            responses.AddRange(firstPage.Objects);

            var pageNumbers = Enumerable.Range(2, firstPage.Paging.AantalPaginas).ToList();

            var tasks = pageNumbers.Select(async i =>
            {
                // waiting for Throttler permission to avoid exceeding the limit
                await Throttler.WaitAsync();

                var task = GetOnePageOfObjects(city, garden, i, PageSize);

                _ = task.ContinueWith(async s =>
                {
                    await Task.Delay(1000 * Period);
                    // reasing one place of Throttler, then the next Http request can be sent
                    Throttler.Release();
                });

                return await task;
            });

            // wait for responses of all Http requests
            var pages = await Task.WhenAll(tasks);

            responses.AddRange(pages.SelectMany(p => p.Objects));

            return responses;
        }
    }

    
}
