using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Suteki.Common.Models;

namespace Suteki.Common.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Use instead of a foreach loop e.g.
        /// MyCollection.Each(item => DoSomething(item));
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (T item in items)
            {
                action(item);
            }
        }

        public static IEnumerable<T> Intersperse<T>(this IEnumerable<T> items, T separator)
        {
            var first = true;
            foreach (var item in items)
            {
                if (first) first = false;
                else
                {
                    yield return separator;
                }
                yield return item;
            }
        }

        public static string Concat(this IEnumerable<string> items)
        {
            return items.Aggregate("", (agg, item) => agg + item);
        }

        /// <summary>
        /// Convenient replacement for a range 'for' loop. e.g. return an array of int from 10 to 20:
        /// int[] tenToTwenty = 10.to(20).ToArray();
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static IEnumerable<int> To(this int from, int to)
        {
            for (int i = from; i <= to; i++)
            {
                yield return i;
            }
        }

        public static void ToConsole<T>(this IEnumerable<T> list)
        {
            list.ForEach(n => Console.Write("{0} ".With(n)));
        }

        public static IEnumerable<T> AtOddPositions<T>(this IEnumerable<T> list)
        {
            bool odd = false; // 0th position is even
            foreach (T item in list)
            {
                odd = !odd;
                if (odd)
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<T> AtEvenPositions<T>(this IEnumerable<T> list)
        {
            bool even = true; // 0th position is even
            foreach (T item in list)
            {
                even = !even;
                if (even)
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<T> ToEnumerable<T>(this T item)
        {
            yield return item;
        }

        public static Money Sum(this IEnumerable<Money> amounts)
        {
            return new Money(amounts.Sum(amount => amount.Amount));
        }

        public static string AsCsv<T>(this IEnumerable<T> items)
            where T : class
        {
            var csvBuilder = new StringBuilder();
            var properties = typeof (T).GetProperties();
            foreach (T item in items)
            {
                string line = properties.Select(p => p.GetValue(item, null).ToCsvValue()).ToArray().Join(",");
                csvBuilder.AppendLine(line);
            }
            return csvBuilder.ToString();
        }

        private static string ToCsvValue<T>(this T item)
        {
			if(item == null)
			{
				return "\"{0}\"".With(item);
			}

            if (item is string)
            {
                return "\"{0}\"".With(item.ToString().Replace("\"", "\\\""));
            }
            double dummy;
            if (double.TryParse(item.ToString(), out dummy))
            {
                return "{0}".With(item);
            }
            return "\"{0}\"".With(item);
        }
    }
}