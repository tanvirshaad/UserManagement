using Microsoft.EntityFrameworkCore;
using UserManagement.Models.Entities;
namespace UserManagement.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Create unique index on Email for non-deleted users
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0")
                .HasDatabaseName("IX_Users_Email_Unique");

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Status)
                    .HasDefaultValue("Active");

                entity.Property(e => e.RegistrationTime)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false);
            });
        }
    }
}
