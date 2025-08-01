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
    }
}
