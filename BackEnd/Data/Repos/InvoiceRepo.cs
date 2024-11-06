using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BackEnd.Models;

namespace BackEnd.Data.Repos
{
    public class InvoiceRepo(ApplicationDbContext context)
    {
        private readonly ApplicationDbContext context = context;

        public async Task<List<Invoice>> GetAllInvoices(){
            IQueryable<Invoice> queryable = context.Invoice.AsQueryable();

            return await queryable.ToListAsync();
        }

        public async Task<Invoice> SaveInvoiceInDb(Invoice invoice){
            context.Add(invoice);
            await context.SaveChangesAsync();
            return invoice;
        }

        public async Task<Invoice?> GetInvoicesById(int id) => await context.Invoice.FindAsync(id);
        public async Task<bool> InvoiceExistsInDb(int id) => await context.Invoice.AnyAsync(x => x.InvoiceId == id);
        public async Task<bool> DeleteInvoice(int id){
            Invoice? invoiceInDb = await GetInvoicesById(id);
            if(invoiceInDb == null){
                return false;
            }
            else{
                context.Remove(invoiceInDb);
                int changesCount = await context.SaveChangesAsync();
                return changesCount == 1;
            }
        }

        public async Task<List<Product>> GetProductsById(Invoice invoice){
            IQueryable<Product> query = context.Product.AsQueryable();
            query = query.Where(p => invoice.ProductIds.Contains(p.ProductId));
            return await query.ToListAsync();
        }

    }
}