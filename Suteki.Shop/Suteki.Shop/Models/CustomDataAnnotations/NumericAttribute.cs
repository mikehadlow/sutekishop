using System.Linq;

namespace Suteki.Shop.Models.CustomDataAnnotations
{
    public class NumericAttribute : StringPropertyAttributeBase
    {
        protected override bool IsValid(string value)
        {
            return !value.Trim().Any(c => !char.IsDigit(c));
        }
    }
}