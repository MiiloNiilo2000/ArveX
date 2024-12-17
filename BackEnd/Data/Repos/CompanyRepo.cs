using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Interfaces;
using BackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Data.Repos
{
    public class CompanyRepo : ICompanyRepo
    {
        private readonly ApplicationDbContext _context;
        public CompanyRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Company>> GetUserCompanies(Profile profile)
        {
            return await _context.Company.Where(u => u.ProfileId == profile.Id)
            .Select(company => new Company
            {
                CompanyId = company.CompanyId,
                Name = company.Name,
                RegisterCode = company.RegisterCode,
                VatNumber = company.VatNumber,
                Address = company.Address,
                PostalCode = company.PostalCode,
                Country = company.Country,
                Email = company.Email,
                Image = company.Image
                IBAN = company.IBAN,
                SWIFT = company.SWIFT,
                Bank = company.Bank,
            }).ToListAsync();
        }

    }
}