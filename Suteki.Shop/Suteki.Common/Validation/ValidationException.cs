using System;
using System.Collections.Generic;
using System.Linq;

namespace Suteki.Common.Validation
{
    [Serializable]
    public class ValidationException : ApplicationException
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

		//TODO: Consider removing these two ctors
		public ValidationException(string message) : base(message) {}
		public ValidationException(string message, Exception inner) : base(message, inner) { }

        public ValidationException(string key, string message) : base(message)
        {
			this.propertyKey = key;
        }
        
		protected ValidationException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

    	public ValidationException(IEnumerable<ValidationException> errors)
    	{
			this.errors = errors.ToArray();
    	}

    	private readonly ValidationException[] errors = new ValidationException[0];
    	private readonly string propertyKey;
    }
}