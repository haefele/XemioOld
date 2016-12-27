using System;

namespace Xemio.Client.Errors
{
    public class ConflictException : XemioException
    {
        public ConflictException()
        {
        }

        public ConflictException(string message) : base(message)
        {
        }

        public ConflictException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}