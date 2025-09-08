using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DoConnect.API.Models;

namespace DoConnect.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) { }

        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Identity tables keys
            builder.Entity<IdentityUserRole<int>>(b => b.HasKey(r => new { r.UserId, r.RoleId }));
            builder.Entity<IdentityUserClaim<int>>(b => b.HasKey(c => c.Id));
            builder.Entity<IdentityUserLogin<int>>(b => b.HasKey(l => new { l.LoginProvider, l.ProviderKey }));
            builder.Entity<IdentityUserToken<int>>(b => b.HasKey(t => new { t.UserId, t.LoginProvider, t.Name }));

            // Vote entity
            builder.Entity<Vote>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.HasOne(e => e.User).WithMany().HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Question).WithMany().HasForeignKey(e => e.QuestionId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Answer).WithMany().HasForeignKey(e => e.AnswerId).OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(e => new { e.UserId, e.QuestionId }).IsUnique()
                      .HasFilter("[QuestionId] IS NOT NULL AND [AnswerId] IS NULL");
                entity.HasIndex(e => new { e.UserId, e.AnswerId }).IsUnique()
                      .HasFilter("[AnswerId] IS NOT NULL AND [QuestionId] IS NULL");
            });

            // UserProfile entity with explicit one-to-one FK configuration
            builder.Entity<UserProfile>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.JoinedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.JoinedAt);
                entity.HasOne(up => up.User)
                      .WithOne(u => u.UserProfile)
                      .HasForeignKey<UserProfile>(up => up.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(e => e.UserId).IsUnique();
            });

            // Notification entity
            builder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Message).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.HasOne(e => e.User).WithMany().HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(e => new { e.UserId, e.IsRead });
                entity.HasIndex(e => e.CreatedAt);
            });

            // User entity with navigation property to UserProfile
            builder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.UserName).IsUnique();
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
                entity.Property(e => e.UserName).IsRequired().HasMaxLength(256);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.HasOne(u => u.UserProfile)
                      .WithOne(up => up.User)
                      .HasForeignKey<UserProfile>(up => up.UserId);
            });

            // Question entity
            builder.Entity<Question>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Content).IsRequired();
                entity.Property(e => e.Topic).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Status).HasDefaultValue(QuestionStatus.Pending);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.HasOne(e => e.User).WithMany(u => u.Questions).HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(e => e.Answers).WithOne(a => a.Question).HasForeignKey(a => a.QuestionId).OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(e => e.Images).WithOne(i => i.Question).HasForeignKey(i => i.QuestionId).OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(e => e.Topic);
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.CreatedAt);
            });

            // Answer entity
            builder.Entity<Answer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Content).IsRequired();
                entity.Property(e => e.Status).HasDefaultValue(AnswerStatus.Pending);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.HasOne(e => e.User).WithMany(u => u.Answers).HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(e => e.Images).WithOne(i => i.Answer).HasForeignKey(i => i.AnswerId).OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.CreatedAt);
            });

            // Image entity
            builder.Entity<Image>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FileName).IsRequired().HasMaxLength(255);
                entity.Property(e => e.FilePath).IsRequired().HasMaxLength(255);
                entity.Property(e => e.ContentType).IsRequired().HasMaxLength(100);
                entity.Property(e => e.UploadedAt).HasDefaultValueSql("GETUTCDATE()");
            });

            // Seed only static Roles
            builder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = "00000000-0000-0000-0000-000000000001" },
                new Role { Id = 2, Name = "User", NormalizedName = "USER", ConcurrencyStamp = "00000000-0000-0000-0000-000000000002" }
            );
        }
    }
}
