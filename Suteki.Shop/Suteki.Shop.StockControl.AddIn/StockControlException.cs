using System;
using System.Runtime.Serialization;

namespace Suteki.Shop.StockControl.AddIn
{
    [Serializable]
    public class StockControlException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public StockControlException(string format, params object[] args) : base(string.Format(format, args))
        {
        }

        public StockControlException()
        {
        }

        public StockControlException(string message) : base(message)
        {
        }

        public StockControlException(string message, Exception inner) : base(message, inner)
        {
        }

        protected StockControlException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}