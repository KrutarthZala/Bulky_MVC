using BulkyBook_WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook_WebAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<CategoryModel> Category { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryModel>(entity =>
            {
                entity.HasKey(e => e.CategoryID);

                entity.ToTable("Categories");

                entity.Property(e => e.CategoryID).HasColumnName("CategoryID");
                entity.Property(e => e.CategoryName).HasMaxLength(int.MaxValue);
                entity.Property(e => e.CategoryOrder).HasColumnName("CategoryOrder");

            });
        }
    }
}
