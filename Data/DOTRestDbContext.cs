using DOTRestAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DOTRestAPI.Data
{
    public class DOTRestDbContext : DbContext
    {
        public DOTRestDbContext(DbContextOptions<DOTRestDbContext> options) : base(options)
        {
        }

        // DbSet untuk masing-masing entity
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
