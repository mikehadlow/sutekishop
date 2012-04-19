namespace Suteki.Shop
{
    public class OrderSearchCriteria
    {
        public int OrderId { get; set; }
        public string Email { get; set; }
        public string Postcode { get; set; }
        public string Lastname { get; set; }
        public int OrderStatusId { get; set; }
    }
}
