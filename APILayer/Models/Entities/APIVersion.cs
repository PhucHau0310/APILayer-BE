using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APILayer.Models.Entities
{
    [Table("APIVersion")]
    public class APIVersion
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ApiId { get; set; }
        public int VersionNumber { get; set; }
        public string? ChangeLog { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string Status { get; set; } = "Active"; //Inactive
                                                       // Relationships
        public API? Api { get; set; }
    }
}
