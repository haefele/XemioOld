using System;

namespace Xemio.Client.Errors
{
    public abstract class XemioException : Exception
    {
        protected XemioException()
        {
        }

        protected XemioException(string message) : base(message)
        {
        }

        protected XemioException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}