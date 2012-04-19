using System;

namespace Suteki.Common.Extensions
{
    /// <summary>
    /// A name value pair with the useful addition that it doesn't work out the value until it's needed
    /// 
    /// Example:
    /// return new NameValue&lt;string, object&gt;("Name", () =&gt; myCustomer.Name);
    /// </summary>
    /// <typeparam name="TName"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class NameValue<TValue>
    {
        public string Name { get; private set; }
        public TValue Value { get { return valueFunction(); } }

        readonly Func<TValue> valueFunction;

        /// <summary>
        /// Example:
        /// return new NameValue&lt;string, object&gt;("Name", () =&gt; myCustomer.Name);
        /// </summary>
        /// <param name="name"></param>
        /// <param name="valueFunction"></param>
        public NameValue(string name, Func<TValue> valueFunction)
        {
            name = RemoveLeadingUnderscoreIfPresent(name);
            Name = name;
            this.valueFunction = valueFunction;
        }

        private static string RemoveLeadingUnderscoreIfPresent(string name)
        {
            if (name.StartsWith("_"))
            {
                return name.Substring(1);
            }
            return name;
        }
    }
}