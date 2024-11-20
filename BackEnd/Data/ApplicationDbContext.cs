using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BackEnd.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace BackEnd.Data
{
    public class ApplicationDbContext : IdentityDbContext<Profile>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
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
        public async Task<bool> UpdateProfile(int id, Profile profile){
            bool isIdsMatch = id.ToString() == profile.Id;
            bool profileExists = await Profile.AnyAsync(x => x.Id == id.ToString());

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
            base.OnModelCreating(modelBuilder); 

            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER"
                }
            };
            modelBuilder.Entity<IdentityRole>().HasData(roles);

            modelBuilder.Entity<Company>()
                .HasKey(c => c.CompanyId);

            modelBuilder.Entity<Company>()
                .HasOne(c => c.profile)
                .WithMany(p => p.Companies)
                .HasForeignKey(c => c.ProfileId);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.company)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CompanyId);
        

        //    modelBuilder.Entity<Product>().HasData(
        //     new Product
        //     {
        //         ProductId = 1,
        //         Name = "Product1",
        //         Description = "Description1",
        //         Price = 100,
        //         CompanyId = 1,
        //         TaxPercent = 22
        //     },
        //     new Product
        //     {
        //         ProductId = 2,
        //         Name = "Product2",
        //         Description = "Description2",
        //         Price = 150,
        //         CompanyId = 1,
        //         TaxPercent = 22
        //     },
        //     new Product
        //     {
        //         ProductId = 3,
        //         Name = "Product3",
        //         Description = "Description3",
        //         Price = 300,
        //         CompanyId = 1,
        //         TaxPercent = 22
        //     },
        //     new Product
        //     {
        //         ProductId = 4,
        //         Name = "Product4",
        //         Description = "Description4",
        //         Price = 400,
        //         CompanyId = 1,
        //         TaxPercent = 22
        //     },
        //     new Product
        //     {
        //         ProductId = 5,
        //         Name = "Product5",
        //         Description = "Description5",
        //         Price = 500,
        //         CompanyId = 1,
        //         TaxPercent = 22
        //     },
        //     new Product
        //     {
        //         ProductId = 6,
        //         Name = "Product6",
        //         Description = "Description6",
        //         Price = 600,
        //         CompanyId = 1,
        //         TaxPercent = 22
        //     },
        //     new Product
        //     {
        //         ProductId = 7,
        //         Name = "Product7",
        //         Description = "Description7",
        //         Price = 700,
        //         CompanyId = 1,
        //         TaxPercent = 22
        //     },
        //     new Product
        //     {
        //         ProductId = 8,
        //         Name = "Product8",
        //         Description = "Description9",
        //         Price = 800,
        //         CompanyId = 1,
        //         TaxPercent = 22
        //     }
        // );
        //     modelBuilder.Entity<Company>().HasData(
        //     new Company
        //     {
        //         CompanyId = 1,
        //         Name = "Firma1",
        //         RegisterCode = 12345,
        //         VatNumber = "EE112",
        //         Address = "Tänav1",
        //         PostalCode = 5432,
        //         Country = "Estonia",
        //         Email = "email@email.com",
        //         ProfileId = "9429539e-5abe-4acf-a932-e747be17b876",
        //     }
        //  );     
        }

    }
}