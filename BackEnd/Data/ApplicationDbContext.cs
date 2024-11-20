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

        public DbSet<CompanyInvoice> CompanyInvoice { get; set; }
        public DbSet<PrivatePersonInvoice> PrivatePersonInvoice { get; set; }
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
           
        }

    }
}