using System;

namespace Xemio.Client.Errors
{
    public class BadRequestException : XemioException
    {
        public BadRequestException()
        {
        }

        public BadRequestException(string message) : base(message)
        {
        }

        public BadRequestException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}