using System.ComponentModel.DataAnnotations;

namespace APILayer.Models.DTOs.Req
{
    public class UserSubsReq
    {
        public int? Id { get; set; }
        public int UserId { get; set; }
        public int ApiId { get; set; }
        public string? SubscriptionType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string PaymentStatus { get; set; } = "Pending";
    }
}
