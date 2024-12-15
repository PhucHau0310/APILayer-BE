using APILayer.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace APILayer.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<UserSubscription> UserSubscriptions { get; set; }
        public DbSet<NewsletterSubscription> NewsletterSubscriptions { get; set; }
        public DbSet<API> APIs { get; set; }
        public DbSet<APIVersion> APIVersions { get; set; }
        public DbSet<APIDocumentation> APIDocumentations { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<FAQ> FAQs { get; set; }
        public DbSet<FeaturedAPI> FeaturedAPIs { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<RefreshTokens> RefreshTokens { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User Entity Configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Email).IsRequired();
                entity.HasIndex(u => u.Email).IsUnique();
            });
            // API Entity Configuration
            modelBuilder.Entity<API>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Name).IsRequired();
                entity.Property(a => a.OwnerId).IsRequired();

                entity.HasOne(a => a.Owner)
                      .WithMany()
                      .HasForeignKey(a => a.OwnerId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // APIDocumentation Entity Configuration
            modelBuilder.Entity<APIDocumentation>()
                .HasKey(d => d.Id);
            modelBuilder.Entity<APIDocumentation>()
                .HasOne(d => d.Api)
                .WithMany(a => a.Documentations)
                .HasForeignKey(d => d.ApiId)
                .OnDelete(DeleteBehavior.Cascade);

            // APIVersion Entity Configuration
            modelBuilder.Entity<APIVersion>()
                .HasKey(v => v.Id);
            modelBuilder.Entity<APIVersion>()
                .HasOne(v => v.Api)
                .WithMany(a => a.Versions)
                .HasForeignKey(v => v.ApiId)
                .OnDelete(DeleteBehavior.Cascade);

            // Payment Entity Configuration
            modelBuilder.Entity<Payment>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.User)
                .WithMany(u => u.Payments)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Api)
                .WithMany()
                .HasForeignKey(p => p.ApiId)
                .OnDelete(DeleteBehavior.Restrict);

            // UserSubscription Entity Configuration
            modelBuilder.Entity<UserSubscription>()
                .HasKey(us => us.Id);
            modelBuilder.Entity<UserSubscription>()
                .HasOne(us => us.User)
                .WithMany(u => u.UserSubscriptions)
                .HasForeignKey(us => us.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Review Entity Configuration
            modelBuilder.Entity<Review>()
                .HasKey(r => r.ReviewId);
            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Api)
                .WithMany(a => a.Reviews)
                .HasForeignKey(r => r.ApiId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Review>()
                .Property(r => r.Rating)
                .HasMaxLength(5);

            // FAQ Entity Configuration
            modelBuilder.Entity<FAQ>()
                .HasKey(f => f.Id);
            modelBuilder.Entity<FAQ>()
                .HasOne(f => f.User)
                .WithMany(u => u.FAQs)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // FeaturedAPI Entity Configuration
            modelBuilder.Entity<FeaturedAPI>()
                .HasKey(fa => fa.Id);
            modelBuilder.Entity<FeaturedAPI>()
                .HasOne(fa => fa.Api)
                .WithMany(a => a.FeaturedAPIs)
                .HasForeignKey(fa => fa.ApiId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<FeaturedAPI>()
                .HasOne(fa => fa.User)
                .WithMany()
                .HasForeignKey(fa => fa.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Notification Entity Configuration
            modelBuilder.Entity<Notification>()
                .HasKey(n => n.Id);

            modelBuilder.Entity<Notification>()
                .HasOne(cm => cm.Sender)
                .WithMany()
                .HasForeignKey(cm => cm.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Notification>()
                .HasOne(cm => cm.Receiver)
                .WithMany()
                .HasForeignKey(cm => cm.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            // NewsletterSubscription Entity Configuration
            modelBuilder.Entity<NewsletterSubscription>()
                .HasKey(ns => ns.Id);
            modelBuilder.Entity<NewsletterSubscription>()
                .HasIndex(ns => ns.Email)
                .IsUnique();

            // ChatMessage Entity Configuration
            modelBuilder.Entity<ChatMessage>()
                .HasKey(cm => cm.Id);

            modelBuilder.Entity<ChatMessage>()
                .HasOne(cm => cm.Sender)
                .WithMany()
                .HasForeignKey(cm => cm.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChatMessage>()
                .HasOne(cm => cm.Recipient)
                .WithMany()
                .HasForeignKey(cm => cm.RecipientId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
