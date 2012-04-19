using System;
using System.Linq;
using System.Text.RegularExpressions;
using Suteki.Common.Extensions;

namespace Suteki.Shop.Models.CustomDataAnnotations
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CreditCardAttribute : StringPropertyAttributeBase
    {
        protected override bool IsValid(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            var trimmedValue = Regex.Replace(value, "[^0-9]", "");
            if (trimmedValue.Length == 0) return false;

            var numbers = trimmedValue.Trim().Reverse().Select(c => int.Parse(c.ToString()));
            var oddSum = numbers.AtOddPositions().Sum();
            var doubleEvenSum = numbers.AtEvenPositions().SelectMany(i => new[] {(i*2)%10, (i*2)/10}).Sum();

            return ((oddSum + doubleEvenSum)%10 == 0);
        }
    }
}