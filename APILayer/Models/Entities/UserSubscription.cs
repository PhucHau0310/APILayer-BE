using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APILayer.Models.Entities
{
    [Table("UserSubscription")]
    public class UserSubscription
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int ApiId { get; set; }
        public string? SubscriptionType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string PaymentStatus { get; set; } = "Pending";

        // Relationships
        public User? User { get; set; }
        public API? Api { get; set; }
    }
}
