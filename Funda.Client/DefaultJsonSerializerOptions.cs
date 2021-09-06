using System.Text.Json;

namespace Funda.Client
{
    internal static class DefaultJsonSerializerOptions
    {
        static DefaultJsonSerializerOptions()
        {
            Instance = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public static JsonSerializerOptions Instance { get; }
    }
}
