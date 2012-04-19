using System.IO;
using System.Net;
using CookComputing.XmlRpc;
using NUnit.Framework;
using Suteki.Shop.XmlRpc;
using System;

namespace Suteki.Shop.Tests.XmlRpc
{
    [TestFixture]
    public class MetaWeblogTests
    {
        /// <summary>
        /// Explicit test for the metaWeblog service. 
        /// If you want to run this, make sure you change the URL, username and password to values that you use.
        /// </summary>
        [Test, Explicit]
        public void GetRecentPosts_ShouldReturnAListOfRecentPosts()
        {
            var metaWeblog = XmlRpcProxyGen.Create<IMetaWeblog>();
            ((IXmlRpcProxy)metaWeblog).Url = "http://localhost:63638/metablogapi.aspx";
            var posts = metaWeblog.getRecentPosts("1", "admin@sutekishop.co.uk", "admin", 10);

            foreach(var post in posts)
            {
                Console.WriteLine(post.title);
            }
        }

        public static void Make_a_publish_request()
        {
            var request = (HttpWebRequest)WebRequest.Create("http://localhost:5401/metablogapi.aspx");

            request.Method = "POST";
            request.ContentType = "text/xml;charset=iso-8859-1";
            request.ContentLength = 981;
            request.Accept = "*/*";
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 9.0; Windows NT 6.1; Windows Live Writer 1.0)";
            request.Host = "localhost";
            //request.Connection = "Close";

            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US, en, *");

            using(var stream = request.GetRequestStream())
            using(var writer = new StreamWriter(stream))
            {
                writer.Write(rawRequest);
            }

            using (var response = request.GetResponse())
            {
                foreach (var header in response.Headers)
                {
                    Console.WriteLine(header.ToString());
                }
                Console.WriteLine();

                using(var stream = response.GetResponseStream())
                using(var reader = new StreamReader(stream))
                {
                    Console.WriteLine(reader.ReadToEnd());
                }
            }
        }

        const string rawRequest =
@"<?xml version=""1.0"" encoding=""iso-8859-1""?>
<methodCall>
 <methodName>metaWeblog.newPost</methodName>
 <params>
  <param>
   <value>
    <string>1</string>
   </value>
  </param>
  <param>
   <value>
    <string>admin@sutekishop.co.uk</string>
   </value>
  </param>
  <param>
   <value>
    <string>admin</string>
   </value>
  </param>
  <param>
   <value>
    <struct>
     <member>
      <name>title</name>
      <value>
       <string>Test Page</string>
      </value>
     </member>
     <member>
      <name>description</name>
      <value>
       <string>&lt;p&gt;Hello, this is a test page, can we see it?&lt;/p&gt;</string>
      </value>
     </member>
     <member>
      <name>categories</name>
      <value>
       <array>
        <data />
       </array>
      </value>
     </member>
    </struct>
   </value>
  </param>
  <param>
   <value>
    <boolean>1</boolean>
   </value>
  </param>
 </params>
</methodCall>";
    
    }
}
