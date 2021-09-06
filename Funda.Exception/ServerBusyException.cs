namespace Funda.Exception
{
    public class ServerBusyException : BaseException
    {
        public override string Message => ErrorResource.ServerBusyError;
    }
}
