using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Suteki.Common.Models;
using Suteki.Common.ViewData;

namespace Suteki.Shop.ViewData
{
    public class ProductViewData : ViewDataBase
    {
        public ProductViewData()
        {
            CategoryIds = new List<int>();
            Sizes = new List<string>();
            ProductImages = new List<ProductImage>();
        }

        public int ProductId { get; set; }
        public int Position { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The product name can not be more than 50 characters")]
        public string Name { get; set; }
        public string UrlName { get; set; }
        public IList<int> CategoryIds { get; set; }
        public int Weight { get; set; }
        public Money Price { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public IList<string> Sizes { get; set; }
        public IList<ProductImage> ProductImages { get; set; }
    }
}