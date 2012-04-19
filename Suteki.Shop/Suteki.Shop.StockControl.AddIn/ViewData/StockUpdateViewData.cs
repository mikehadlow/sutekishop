using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Suteki.Shop.StockControl.AddIn.ViewData
{
    public class StockUpdateViewData
    {
        public StockUpdateViewData()
        {
            UpdateItems = new List<UpdateItem>();
        }

        public string ReturnUrl { get; set; }
        public string Comment { get; set; }
        public IList<UpdateItem> UpdateItems { get; set; }
    }

    public class UpdateItem
    {
        [Required]
        public int StockItemId { get; set; }
        public string Received { get; set; }
        public string Adjustment { get; set; }
        public bool IsInStock { get; set; }
        
        public bool HasReceivedValue()
        {
            int outValue;
            return (Received != null) && int.TryParse(Received, out outValue);
        }

        public bool HasAdjustedValue()
        {
            int outValue;
            return (Adjustment != null) && int.TryParse(Adjustment, out outValue);
        }

        public int GetReceivedValue()
        {
            return int.Parse(Received);
        }

        public int GetAdjustmentValue()
        {
            return int.Parse(Adjustment);
        }
    }
}