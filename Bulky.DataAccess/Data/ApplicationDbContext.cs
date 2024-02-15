﻿using BulkyBook.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<ProductModel> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryModel>().HasData(
                new CategoryModel { CategoryID=1, CategoryName="Action", CategoryOrder=1 },
                new CategoryModel { CategoryID=2, CategoryName="SciFi" , CategoryOrder=2 },
                new CategoryModel { CategoryID=3, CategoryName="Comics", CategoryOrder=3 }
                );

            modelBuilder.Entity<ProductModel>().HasData(
                new ProductModel
                { 
                    ProductID = 1, 
                    ProductTitle = "Fortune of Time", 
                    ProductAuthor="Billy Spark", 
                    ProductDescription= "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincProductIDunt. ",
                    ProductISBN="SWD9999001",
                    ProductListPrice=99,
                    ProductPrice=90,
                    ProductPrice50=85,
                    ProductPrice100=80,
                    CategoryID =1,
                    ProductImageURL=""
                },
                new ProductModel
                {
                    ProductID = 2,
                    ProductTitle = "Dark Skies",
                    ProductAuthor = "Nancy Hoover",
                    ProductDescription = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincProductIDunt. ",
                    ProductISBN = "CAW777777701",
                    ProductListPrice = 40,
                    ProductPrice = 30,
                    ProductPrice50 = 25,
                    ProductPrice100 = 20,
                    CategoryID = 1,
                    ProductImageURL = ""
                },
                new ProductModel
                {
                    ProductID = 3,
                    ProductTitle = "Vanish in the Sunset",
                    ProductAuthor = "Julian Button",
                    ProductDescription = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincProductIDunt. ",
                    ProductISBN = "RITO5555501",
                    ProductListPrice = 55,
                    ProductPrice = 50,
                    ProductPrice50 = 40,
                    ProductPrice100 = 35,
                    CategoryID = 1,
                    ProductImageURL = ""
                },
                new ProductModel
                {
                    ProductID = 4,
                    ProductTitle = "Cotton Candy",
                    ProductAuthor = "Abby Muscles",
                    ProductDescription = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincProductIDunt. ",
                    ProductISBN = "WS3333333301",
                    ProductListPrice = 70,
                    ProductPrice = 65,
                    ProductPrice50 = 60,
                    ProductPrice100 = 55,
                    CategoryID = 2,
                    ProductImageURL = ""
                },
                new ProductModel
                {
                    ProductID = 5,
                    ProductTitle = "Rock in the Ocean",
                    ProductAuthor = "Ron Parker",
                    ProductDescription = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincProductIDunt. ",
                    ProductISBN = "SOTJ1111111101",
                    ProductListPrice = 30,
                    ProductPrice = 27,
                    ProductPrice50 = 25,
                    ProductPrice100 = 20,
                    CategoryID = 2,
                    ProductImageURL = ""
                },
                new ProductModel
                {
                    ProductID = 6,
                    ProductTitle = "Leaves and Wonders",
                    ProductAuthor = "Laura Phantom",
                    ProductDescription = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincProductIDunt. ",
                    ProductISBN = "FOT000000001",
                    ProductListPrice = 25,
                    ProductPrice = 23,
                    ProductPrice50 = 22,
                    ProductPrice100 = 20,
                    CategoryID = 3,
                    ProductImageURL = ""
                }
                );
        }
    }
}
