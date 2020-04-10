using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            //Contacts
            modelBuilder.Entity<Contact>().HasData(
            new Contact() { Id = 1, Name = "Võ Trung Hiếu", Email = "hieuvo044@gmail.com", Message = "Very good" },
            new Contact() { Id = 2, Name = "Phuong Quyen", Email = "hieuvo044@gmail.com", Message = "Very good" },
            new Contact() { Id = 3, Name = "Võ Trung Hiếu", Email = "hieuvo044@gmail.com", Message = "Very good" }
            );
            //Languages
            modelBuilder.Entity<Language>().HasData(
                new Language() { Id = 1, Name = Enums.Languages.VIETNAM },
                new Language() { Id = 2, Name = Enums.Languages.ENGLISH }
            );
            //Promotions
            modelBuilder.Entity<Promotion>().HasData(
              new Promotion() { Id = 1, Code = "123", DiscountAmount = 10000 },
              new Promotion() { Id = 2, Code = "1234", DiscountAmount = 20000 }
            );
            //Categories
            modelBuilder.Entity<Category>().HasData(
                new Category()
                {
                    Id = 1,
                    IsShowOnHome = true,
                    Status = Enums.CategoryStatus.Active
                }
           );
            //CategoryTranslations
           modelBuilder.Entity<CategoryTranslation>().HasData(
                 new CategoryTranslation() { Name = "Bánh ngọt", LanguageId = 1, CategoryUrl = "banh-ngot", CategoryId = 1 },
                 new CategoryTranslation() { Name = "Cake", LanguageId = 2, CategoryUrl = "cake", CategoryId = 1 }
            );
            //Products
            modelBuilder.Entity<Product>().HasData(
                 new Product()
                 {
                     Id = 1,
                     Price = 20000,
                     OriginalPrice = 17000,
                     CategoryId=1
                 }
            );
            //ProductTranslations
            modelBuilder.Entity<ProductTranslation>().HasData(
                new ProductTranslation() { ProductId = 1, Name = "Bánh ngọt1", LanguageId = 1, ProductUrl = "banh-ngot1", Description = "This is banh ngot 1" },
                new ProductTranslation() { ProductId = 1, Name = "Cake1", LanguageId = 2, ProductUrl = "cake1", Description = "This is cake1" }
            );
            //ProductImages
            modelBuilder.Entity<ProductImage>().HasData(
               new ProductImage() { ProductId = 1, Id = 1, ImagePath = "http://product.hstatic.net/1000026716/product/81ax00mcvn_bd76b8bf0aed4307bc9714e4dc5830f0_large.jpg", Caption = "!23", IsDefault = true },
               new ProductImage() { ProductId = 1, Id = 2, ImagePath = "http://product.hstatic.net/1000026716/product/81ax00mcvn_bd76b8bf0aed4307bc9714e4dc5830f0_large.jpg", Caption = "!23", IsDefault = false }
            );
            //Carts
            modelBuilder.Entity<Cart>().HasData(
                new Cart()
                {
                    Id = 1,
                    Price = 20000
                }
           );
            //CartProducts
            modelBuilder.Entity<CartProduct>().HasData(
                 new CartProduct() { ProductID = 1, Quantity = 2,CartID=1 }
            );


        }
    }

}