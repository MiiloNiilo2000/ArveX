using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Data;
using BackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Data.Repos
{
    public class ProductsRepo
    {
        private readonly ApplicationDbContext _context;

        public ProductsRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetProductsByIds(List<int> productIds)
        {
            return await _context.Product
                .Where(p => productIds.Contains(p.ProductId))
                .ToListAsync();
        }
    }
}