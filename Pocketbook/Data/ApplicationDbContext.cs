using Microsoft.EntityFrameworkCore;
using Pocketbook.Models;

namespace Pocketbook.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)  { }
    }
}