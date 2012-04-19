using System.ComponentModel.DataAnnotations;

namespace Suteki.Shop.Models.CustomDataAnnotations
{
    public class EmailAttribute : RegularExpressionAttribute
    {
        public EmailAttribute()
            : base(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*")
        {
        }
    }
}