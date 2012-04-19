using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Suteki.Common.Models
{
    /// <summary>
    /// Useful for assigning int collections to SelectLists
    /// </summary>
    public static class StringValueExtensions
    {
        public static IEnumerable<StringValue> ToStringValues(this IEnumerable<int> ints)
        {
            return ints.Select(i => new StringValue(i.ToString("00")));
        }

        public static IEnumerable<StringValue> AddBlankFirstValue(this IEnumerable<StringValue> values)
        {
            return new List<StringValue> {new StringValue("0", "")}.Union(values);
        }

        public static SelectList AsSelectList(this IEnumerable<StringValue> values)
        {
            return values.AsSelectList(null);
        }

        public static SelectList AsSelectList(this IEnumerable<StringValue> values, object selectedValue)
        {
            return new SelectList(values, "Value", "Text", selectedValue);
        }
    }

    public class StringValue
    {
        public StringValue(string value)
        {
            Value = value;
            Text = value;
        }

        public StringValue(string value, string text)
        {
            Value = value;
            Text = text;
        }

        public string Value { get; private set; }
        public string Text { get; private set; }
    }
}