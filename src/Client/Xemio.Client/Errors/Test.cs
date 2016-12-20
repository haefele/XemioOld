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

    public class UnauthorizedException : XemioException
    {
        public UnauthorizedException()
        {
        }

        public UnauthorizedException(string message) : base(message)
        {
        }

        public UnauthorizedException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}