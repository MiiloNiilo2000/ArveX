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
                ProfileId = "1"
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
                ProfileId = "2",
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
                ProfileId = "1",
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
                ProfileId = "2",
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

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.Id);
        });
        
        modelBuilder.Entity<Profile>().HasData(
            new Profile
            {
                Id = "1",
                //Username = "Profiil1",
               // Password = "9m3hoCPjb1UPf9Rtjv5k9Rd/Qe3eV03FWdj8gZ+CY8I=", //Password1
                Email = "Profiil1@mail.ee",
            },
            new Profile
            {
                Id = "2",
                //Username = "Profiil2",
                //Password = "Nap22SGtaVHh4mEwqD9K/Ew/g7YFYYv8VxHOL5D3nO4=", //Password2
                Email = "Profiil2@mail.ee",
            }
        );
        }

    }
}