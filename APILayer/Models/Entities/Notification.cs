using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APILayer.Models.Entities
{
    [Table("Notification")]
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; } // NULL nếu là thông báo toàn hệ thống
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Message { get; set; }
        public string? Type { get; set; } // info, warning, success, error
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }

        // Relationships
        public User? User { get; set; }
    }
}
