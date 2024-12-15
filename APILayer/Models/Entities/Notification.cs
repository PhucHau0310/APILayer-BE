using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APILayer.Models.Entities
{
    [Table("Notification")]
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        public string? Message { get; set; }
        public bool? IsRead { get; set; } = false;
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public int SenderId { get; set; }

        [ForeignKey("SenderId")]
        public User? Sender { get; set; }

        [Required]
        public int ReceiverId { get; set; }

        [ForeignKey("ReceiverId")]
        public User? Receiver { get; set; }
    }
}
