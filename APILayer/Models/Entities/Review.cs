using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APILayer.Models.Entities
{
    [Table("Review")]
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int ApiId { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime ReviewDate { get; set; }

        // Relationships
        public User? User { get; set; }
        public API? Api { get; set; }
    }
}
