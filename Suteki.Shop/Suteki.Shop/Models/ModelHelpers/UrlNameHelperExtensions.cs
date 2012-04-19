using System;
using System.Text.RegularExpressions;

namespace Suteki.Shop.Models.ModelHelpers
{
    public static class UrlNameHelperExtensions
    {
        /// <summary>
        /// Replaces any non-alphanumeric character with an underscore
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToUrlFriendly(this string text)
        {
            return Regex.Replace(text, @"[^A-Za-z0-9]", "_");
        }
    }
}
