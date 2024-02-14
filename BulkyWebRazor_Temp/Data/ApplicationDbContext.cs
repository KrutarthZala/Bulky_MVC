using BulkyBookWebRazor_Temp.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkyBookWebRazor_Temp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<CategoryModel> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryModel>().HasData(
                new CategoryModel { CategoryID=1, CategoryName="Action", CategoryOrder=1},
                new CategoryModel { CategoryID = 2, CategoryName = "SciFi", CategoryOrder = 2 },
                new CategoryModel { CategoryID = 3, CategoryName = "Comics", CategoryOrder = 3 }
                );
        }
    }
}
