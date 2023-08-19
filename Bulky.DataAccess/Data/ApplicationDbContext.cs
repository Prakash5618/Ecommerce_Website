using Microsoft.EntityFrameworkCore;
using Bulky.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Bulky.DataAccess.Data
{
    public class ApplicationDbContext :IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)

        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name="Pressure Cooker", DisplayOrder=1},
                new Category { Id = 2, Name = "Gas Stove", DisplayOrder = 2 });

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Title = "Pressure Cooker",
                    Description = "Prestige Svachh Popular Spillage Control Stainless Steel Pressure Cooker Silver",
                    Model = "Outer Lid Stainless Steel",
                    ListPrice = 2399.00,
                    Price = 1790.00,
                    Price50 = 1520.00,
                    Price100 = 999.00,
                    CategoryId = 1,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 2,
                    Title = "Cook Tops",
                    Description = "Prestige Svachh Glass Top Gas Stove 2 Burners with Liftable Burners",
                    Model = "Glasstop",
                    ListPrice = 9695.00,
                    Price = 8499.00,
                    Price50 = 7270.00,
                    Price100 = 6499.00,
                    CategoryId = 2,
                    ImageUrl =  ""
                }); ;
        }

    }
}
