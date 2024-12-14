using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APILayer.Models.Entities
{
    [Table("FeaturedAPI")]
    public class FeaturedAPI
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ApiId { get; set; }
        [Required]
        public int UserId { get; set; }
        public string? ReasonForFeature { get; set; }
        public DateTime FeaturedFrom { get; set; }
        public DateTime FeaturedTo { get; set; }

        // Relationships
        public API? Api { get; set; }
        public User? User { get; set; }
    }
}
