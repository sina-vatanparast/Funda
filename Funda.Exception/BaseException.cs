using System;

namespace Funda.Exception
{
    public abstract class BaseException : System.Exception
    {
        public override string Message => throw new NotImplementedException();
    }
}