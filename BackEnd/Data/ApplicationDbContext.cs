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
        public DbSet<Profile> Profile {get; set; } 
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
        public async Task<bool> UpdateCompany(int Id, Company company){
            bool isIdsMatch = Id == company.CompanyId;
            bool companyExists = await Company.AnyAsync(x => x.CompanyId == Id);

            if (!isIdsMatch || !companyExists)
            {
                return false;
            }

            Update(company);
            int updatedRecordsCount = await SaveChangesAsync();
            return updatedRecordsCount == 1;
        }
        public async Task<bool> UpdateProfile(int Id, Profile profile){
            bool isIdsMatch = Id == profile.ProfileId;
            bool profileExists = await Profile.AnyAsync(x => x.ProfileId == Id);

            if (!isIdsMatch || !profileExists)
            {
                return false;
            }

            Update(profile);
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
                Email = "example@company.com",
                ProfileId = 1
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
                Email = "example2@company.com",
                ProfileId = 2,
            },
            new Company
            {
                CompanyId = 3,
                Name = "Example Company 3",
                RegisterCode = 123446,
                VatNumber = "EE1234567889",
                Address = "Example Address 3",
                PostalCode = 1234456,
                Country = "Estonia",
                Email = "example3@company.com",
                ProfileId = 1,
            },
            new Company
            {
                CompanyId = 4,
                Name = "Example Company 4",
                RegisterCode = 65432,
                VatNumber = "EE123457678",
                Address = "Example Address 4",
                PostalCode = 556134,
                Country = "Estonia",
                Email = "example3@company.com",
                ProfileId = 2,
            }
        );
            modelBuilder.Entity<Product>().HasData(
            new Product
            {
                ProductId = 1,
                Name = "Product1",
                Description = "Description1",
                Price = 100,
                CompanyId = 1,
                TaxPercent = 22
            },
            new Product
            {
                ProductId = 2,
                Name = "Product2",
                Description = "Description2",
                Price = 150,
                CompanyId = 1,
                TaxPercent = 22
            }
        );
        modelBuilder.Entity<Company>()
            .HasOne(p => p.profile)
            .WithMany(c => c.Companies)
            .HasForeignKey(p => p.ProfileId);

        modelBuilder.Entity<Profile>().HasData(
            new Profile
            {
                ProfileId = 1,
                Username = "Profiil1",
                Email = "Profiil1@mail.ee",
            },
            new Profile
            {
                ProfileId = 2,
                Username = "Profiil2",
                Email = "Profiil2@mail.ee",
            }
        );
        }

    }
}