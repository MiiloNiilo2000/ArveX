using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BackEnd.Models;


namespace BackEnd.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options){

        }

        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Company> Company { get; set; }
        public async Task<bool> UpdateProduct(int id, Product product){
            bool isIdsMatch = id == product.ProductId;
            bool productExists = await Product.AnyAsync(x => x.ProductId == id);

            if (!isIdsMatch || !productExists)
            {
                return false;
            }

            Update(product);
            int updatedRecordsCount = await SaveChangesAsync();
            return updatedRecordsCount == 1;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasOne(p => p.company)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CompanyId);

            modelBuilder.Entity<Company>().HasData(
            new Company
            {
                CompanyId = 1,
                Name = "Example Company",
                RegisterCode = 12345,
                VatNumber = "EE123456789",
                Address = "Example Address",
                PostalCode = 12345,
                Country = "Estonia",
                Email = "example@company.com"
            },
            new Company
            {
                CompanyId = 2,
                Name = "Example Company 2",
                RegisterCode = 12344,
                VatNumber = "EE123456788",
                Address = "Example Address 2",
                PostalCode = 12344,
                Country = "Estonia",
                Email = "example2@company.com"
            }
        );
            modelBuilder.Entity<Product>().HasData(
            new Product
            {
                ProductId = 1,
                Name = "Product1",
                Description = "Description1",
                Price = 100,
                CompanyId = 1
            },
            new Product
            {
                ProductId = 2,
                Name = "Product2",
                Description = "Description2",
                Price = 150,
                CompanyId = 1
            }
        );
        }

    }
}