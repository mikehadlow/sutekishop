using System;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Suteki.Common.Services;

namespace Suteki.Common.Extensions
{
    //
    // Taken from Troy Goode's blog http://www.squaredroot.com/post/2008/06/MVC-and-SSL.aspx
    //
    //
    public class UrlExtensionsHelper
    {
        /// <summary>
        /// Takes a relative or absolute url and returns the fully-qualified url path.
        /// </summary>
        /// <param name="text">The url to make fully-qualified. Ex: Home/About</param>
        /// <returns>The absolute url plus protocol, server, & port. Ex: http://localhost:1234/Home/About</returns>
        public virtual string ToFullyQualifiedUrl(string text)
        {

            //### the VirtualPathUtility doesn"t handle querystrings, so we break the original url up
            var oldUrl = text;
            var oldUrlArray = (oldUrl.Contains("?") ? oldUrl.Split('?') : new[] { oldUrl, "" });

            //### we"ll use the Request.Url object to recreate the current server request"s base url
            //### requestUri.AbsoluteUri = "http://domain.com:1234/Home/Index?page=123"
            //### requestUri.LocalPath = "/Home/Index"
            //### requestUri.Query = "?page=123"
            //### subtract LocalPath and Query from AbsoluteUri and you get "http://domain.com:1234", which is urlBase
            var requestUri = GetRequestUri();
            var localPathAndQuery = requestUri.LocalPath + requestUri.Query;
            var urlBase = requestUri.AbsoluteUri.Substring(0, requestUri.AbsoluteUri.Length - localPathAndQuery.Length);

            //### convert the request url into an absolute path, then reappend the querystring, if one was specified
            var newUrl = VirtualPathUtility.ToAbsolute(oldUrlArray[0]);
            if (!string.IsNullOrEmpty(oldUrlArray[1]))
                newUrl += "?" + oldUrlArray[1];

            //### combine the old url base (protocol + server + port) with the new local path
            return urlBase + newUrl;
        }

        public virtual Uri GetRequestUri()
        {
            return HttpContext.Current.Request.Url;
        }

        /// <summary>
        /// Looks for Html links in the passed string and turns each relative or absolute url and returns the fully-qualified url path.
        /// </summary>
        /// <param name="text">The url to make fully-qualified. Ex: <a href="Home/About">Blah</a></param>
        /// <returns>The absolute url plus protocol, server, & port. Ex: <a href="http://localhost:1234/Home/About">Blah</a></returns>
        public virtual string ToFullyQualifiedLink(string text)
        {

            var regex = new Regex(
                "(?<Before><a.*href=\")(?!http)(?<Url>.*?)(?<After>\".+>)",
                RegexOptions.Multiline | RegexOptions.IgnoreCase
            );

            return regex.Replace(text, m =>
                m.Groups["Before"].Value +
                ToFullyQualifiedUrl(m.Groups["Url"].Value) +
                m.Groups["After"].Value
            );

        }

        /// <summary>
        /// Takes a relative or absolute url and returns the fully-qualified url path using the Https protocol.
        /// </summary>
        /// <param name="text">The url to make fully-qualified. Ex: Home/About</param>
        /// <returns>The absolute url plus server, & port using the Https protocol. Ex: https://localhost:1234/Home/About</returns>
        public virtual MvcHtmlString ToSslUrl(MvcHtmlString text)
        {
            // TODO: This won't work with .NET 4
            if (!UseSsl()) return text;
            return MvcHtmlString.Create(ToFullyQualifiedUrl(text.ToString()).Replace("http:", "https:"));
        }

        public virtual string ToSslUrl(string text)
        {
            if (!UseSsl()) return text;
            return ToFullyQualifiedUrl(text).Replace("http:", "https:");
        }

        /// <summary>
        /// Looks for Html links in the passed string and turns each relative or absolute url into a fully-qualified url path using the Https protocol.
        /// </summary>
        /// <param name="text">The url to make fully-qualified. Ex: <a href="Home/About">Blah</a></param>
        /// <returns>The absolute url plus server, & port using the Https protocol. Ex: <a href="https://localhost:1234/Home/About">Blah</a></returns>
        public virtual MvcHtmlString ToSslLink(MvcHtmlString text)
        {
            // TODO: This won't work with .NET 4
            if (!UseSsl()) return text;
            return MvcHtmlString.Create(ToFullyQualifiedLink(text.ToString()).Replace("http:", "https:"));
        }

        public virtual bool UseSsl()
        {
			var appSettings = new AppSettings();
			return "true".Equals(appSettings.GetSetting(AppSettings.UseSsl)); 
        }
    }
}