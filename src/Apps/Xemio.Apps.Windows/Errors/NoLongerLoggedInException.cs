using System;
using Xemio.Client.Errors;

namespace Xemio.Apps.Windows.Errors
{
    public class NoLongerLoggedInException : XemioException
    {
        public NoLongerLoggedInException()
        {
        }

        public NoLongerLoggedInException(string message) : base(message)
        {
        }

        public NoLongerLoggedInException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}