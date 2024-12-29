using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace APILayer.Models.Entities
{
    [Table("ChatMessage")]
    public class ChatMessage
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int? SenderId { get; set; }
        [Required]
        public int? RecipientId { get; set; }
        [ForeignKey("SenderId")]
        public User? Sender { get; set; }
        [ForeignKey("RecipientId")]
        public User? Recipient { get; set; }
        public string? Message { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
