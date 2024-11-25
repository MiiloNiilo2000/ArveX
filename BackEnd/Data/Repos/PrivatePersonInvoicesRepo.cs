using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BackEnd.Models;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace BackEnd.Data.Repos
{
    public class PrivatePersonInvoicesRepo : InvoicesRepoBase<PrivatePersonInvoice>
    {
        public PrivatePersonInvoicesRepo(ApplicationDbContext context) : base(context) { }

        public async Task<List<Product>> GetProductsByIds(List<int> productIds)
        {
            IQueryable<Product> query = context.Product.AsQueryable();
            query = query.Where(p => productIds.Contains(p.ProductId));
            return await query.ToListAsync();
        }
    }
}