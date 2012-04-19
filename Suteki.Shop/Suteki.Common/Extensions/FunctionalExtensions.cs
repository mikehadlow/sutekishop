using System;

namespace Suteki.Common.Extensions
{
    public static class FunctionalExtensions
    {
        /// <summary>
        /// http://mikehadlow.blogspot.com/2010/09/more-boilerplate-code-removal.html
        /// </summary>
        public static void ApplyTo<T>(this T arg, params Action<T>[] actions)
        {
            Array.ForEach(actions, action => action(arg));
        }
    }
}