using System;
using System.Linq;
using System.Runtime.Serialization;
using Suteki.Common.Extensions;

namespace Suteki.Shop.Repositories
{
    public static class UrlNamedExtensions
    {
        public static T WithUrlName<T>(this IQueryable<T> items, string urlName) where T : IUrlNamed
        {
            var item = items
                .SingleOrDefault(i => i.UrlName == urlName);

            if (item == null) throw new UrlNameNotFoundException("Unknown UrlName '{0}' for type {1}"
                .With(urlName, typeof(T).FullName));
            return item;
        }
    }

    [Serializable]
    public class UrlNameNotFoundException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public UrlNameNotFoundException()
        {
        }

        public UrlNameNotFoundException(string message) : base(message)
        {
        }

        public UrlNameNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        protected UrlNameNotFoundException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
