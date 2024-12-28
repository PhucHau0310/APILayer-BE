namespace APILayer.Models.DTOs.Req
{
    public class APIReq
    {
        public int OwnerId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public string? PricingUrl { get; set; }
        public decimal BasePrice { get; set; }
        public string Status { get; set; } = "Active"; // Inactive
        public int OverallSubscription { get; set; } = 0;
    }
}
