using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APILayer.Models.Entities
{
    [Table("API")]
    public class API
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int OwnerId { get; set; }
        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set;}
        public string? PricingUrl { get; set;}
        public decimal BasePrice { get; set; }
        public string Status { get; set; } = "Active"; // Inactive
        public int OverallSubscription { get; set; }
        public DateTime CreateAt { get; set; }

        // Relationships
        public User? Owner { get; set; }
        public ICollection<APIVersion>? Versions { get; set; }
        public ICollection<APIDocumentation>? Documentations { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<FeaturedAPI>? FeaturedAPIs { get; set; }
    }
}
