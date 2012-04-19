using System;
using NUnit.Framework;

namespace Suteki.Shop.Tests.Spikes
{
    [TestFixture]
    public class UriSpike
    {
        [Test]
        public void Uri_ShouldBeAbleToGetPartsOfAURL()
        {
            Uri url = new Uri("http://www.mydomain.com/folder/page.aspx");
            Console.WriteLine("url.AbsoluteUri: '{0}'", url.AbsoluteUri);
            Console.WriteLine("url.Host {0}", url.Host);
            Console.WriteLine("url.Scheme {0}", url.Scheme);
            Console.WriteLine("url.AbsolutePath {0}", url.AbsolutePath);
            Console.WriteLine("url.Authority {0}", url.Authority);
            Console.WriteLine("url.LocalPath {0}", url.LocalPath);
        }
    }
}
