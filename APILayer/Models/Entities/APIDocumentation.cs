using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APILayer.Models.Entities
{
    [Table("APIDocumentation")]
    public class APIDocumentation
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ApiId { get; set; }
        [Required]
        public string? DocContent { get; set; }
        public string? IntegrationGuide { get; set; }
        public string? CodeExamples { get; set; }
        public string? Status { get; set; }

        // Relationships
        public API? Api { get; set; }
    }
}
