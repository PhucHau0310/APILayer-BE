using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APILayer.Models.Entities
{
    [Table("FAQ")]
    public class FAQ
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Question { get; set; }
        public string? Answer { get; set; }
        public string? Category { get; set; }

        // Relationships
        [Required]
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
