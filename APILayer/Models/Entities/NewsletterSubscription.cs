using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APILayer.Models.Entities
{
    [Table("NewsletterSubscription")]
    public class NewsletterSubscription
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        public DateTime SubscribedAt { get; set; }
        public string? Status { get; set; }
    }
}
