using System;
using System.Runtime.Serialization;

namespace Suteki.Common
{
    [Serializable]
    public class SutekiCommonException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public SutekiCommonException(string format, params object[] args) : base(string.Format(format, args))
        {
        }

        public SutekiCommonException()
        {
        }

        public SutekiCommonException(string message) : base(message)
        {
        }

        public SutekiCommonException(string message, Exception inner) : base(message, inner)
        {
        }

        protected SutekiCommonException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}