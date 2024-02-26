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
            #region Category Table Mapping
            modelBuilder.Entity<CategoryModel>(entity =>
            {
                entity.HasKey(e => e.CategoryID);

                entity.ToTable("Categories");

                entity.Property(e => e.CategoryID).HasColumnName("CategoryID");
                entity.Property(e => e.CategoryName).HasMaxLength(int.MaxValue);
                entity.Property(e => e.CategoryOrder).HasColumnName("CategoryOrder");

            });
            #endregion

            #region Product Table Mapping
            modelBuilder.Entity<ProductModel>(entity =>
            {
                // Configure primary key
                entity.HasKey(e => e.ProductID);

                entity.ToTable("Products");

                // Configure properties
                entity.Property(e => e.ProductTitle).HasMaxLength(255);
                entity.Property(e => e.ProductDescription).HasMaxLength(500);
                entity.Property(e => e.ProductISBN).HasMaxLength(50);
                entity.Property(e => e.ProductAuthor).HasMaxLength(100);
                entity.Property(e => e.ProductListPrice).HasColumnType("double");
                entity.Property(e => e.ProductPrice).HasColumnType("double");
                entity.Property(e => e.ProductPrice50).HasColumnType("double");
                entity.Property(e => e.ProductPrice100).HasColumnType("double");
                entity.Property(e => e.ProductImageURL).HasMaxLength(255);

                // Configure relationships
                entity.HasOne(e => e.Category)
                      .WithMany()
                      .HasForeignKey(e => e.CategoryID)
                      .IsRequired(false); // Assuming Category is optional
            });
            #endregion

            #region Company Table Mapping
            modelBuilder.Entity<CompanyModel>(entity =>
            {
                entity.ToTable("Companies");

                entity.HasKey(e => e.CompanyID);
                entity.Property(e => e.CompanyName).HasMaxLength(255);
                entity.Property(e => e.CompanyStreetAddress).HasMaxLength(255);
                entity.Property(e => e.CompanyCity).HasMaxLength(100);
                entity.Property(e => e.CompanyState).HasMaxLength(2);
                entity.Property(e => e.CompanyPostalCode).HasMaxLength(10);
                entity.Property(e => e.CompanyPhoneNumber).HasMaxLength(20);
            });
            #endregion
        }
    }
}
