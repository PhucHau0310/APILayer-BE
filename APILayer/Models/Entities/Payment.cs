using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APILayer.Models.Entities
{
    [Table("Payment")]
    public class Payment
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int ApiId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = "VNPay";
        public string PaymentStatus { get; set; } = "Pending";
        public DateTime PaymentDate { get; set; }

        // Relationships
        public required User User { get; set; }
        public required API Api { get; set; }
    }
}
