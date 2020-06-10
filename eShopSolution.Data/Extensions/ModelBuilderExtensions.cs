using eShopSolution.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

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
                new Language() { Id = "vn", Name = "VIETNAM" },
                new Language() { Id = "en", Name = "ENGLISH" }
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
                 new CategoryTranslation() { Name = "Bánh ngọt", LanguageId = "vn", CategoryUrl = "banh-ngot", CategoryId = 1 },
                 new CategoryTranslation() { Name = "Cake", LanguageId = "en", CategoryUrl = "cake", CategoryId = 1 }
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
                new ProductTranslation() { ProductId = 1, Name = "Bánh ngọt1", LanguageId = "vn", ProductUrl = "banh-ngot1", Description = "This is banh ngot 1" },
                new ProductTranslation() { ProductId = 1, Name = "Cake1", LanguageId = "en", ProductUrl = "cake1", Description = "This is cake1" }
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
            // any guid
            var roleId = new Guid("8D04DCE2-969A-435D-BBA4-DF3F325983DC");
            var adminId = new Guid("69BD714F-9576-45BA-B5B7-F00649BE00DE");
            modelBuilder.Entity<RoleApp>().HasData(
                new RoleApp
                    {
                        Id = roleId,
                        Name = "admin",
                        NormalizedName = "admin",
                        Description = "Administrator role"
                    },
                 new RoleApp
                 {
                     Id = new Guid("8D04DCE2-969A-435D-BBA4-DF3F325983DD"),
                     Name = "client",
                     NormalizedName = "client",
                     Description = "Client role"
                 }
            );

            var hasher = new PasswordHasher<UserApp>();
            modelBuilder.Entity<UserApp>().HasData(new UserApp
            {
                Id = adminId,
                UserName = "admin",
                NormalizedUserName = "admin",
                Email = "hieuvo044@gmail.com",
                NormalizedEmail = "hieuvo044@gmail.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "123"),
                SecurityStamp = string.Empty,
                Dob = new DateTime(2020, 01, 31),
                RoleID= roleId
            });
            modelBuilder.Entity<Cart>().HasData(new Cart
            {
                Id = 2,
                UserId = adminId,   
                Price=0,
                Created_At = DateTime.Now,
                CartProducts = new List<CartProduct>(),
            });
        }
    }

}