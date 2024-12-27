
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APILayer.Models.Entities
{
    [Table("User")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set;}
        [Required]
        [MaxLength(50)]
        public string? Username { get; set;}
        [Required]
        [MaxLength(250)]
        public string? HashedPassword { get; set;}
        public string? Avatar { get; set;}
        public string Role { get; set; } = "Customer"; // Provider, Admin
        public bool IsEmailConfirmed { get; set; } = false;
        public string? EmailConfirmationToken { get; set; }
        public string? Provider { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool IsActive { get; set; } = true;

        // Relationships
        public ICollection<RefreshTokens> RefreshTokens { get; set; } = new List<RefreshTokens>();
        public ICollection<Payment>? Payments { get; set; }
        public ICollection<UserSubscription>? UserSubscriptions { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<FAQ>? FAQs { get; set; }
    }
}
