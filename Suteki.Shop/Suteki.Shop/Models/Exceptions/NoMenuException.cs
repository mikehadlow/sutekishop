using System;

namespace Suteki.Shop.Models.Exceptions
{
    public class NoMenuException : ApplicationException
    {
        public NoMenuException(string message) : base(message)
        {}
    }
}