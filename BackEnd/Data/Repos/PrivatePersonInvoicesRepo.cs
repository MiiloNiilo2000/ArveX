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
    public class PrivatePersonInvoicesRepo
    {
        private readonly ApplicationDbContext context;

        public PrivatePersonInvoicesRepo(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<List<PrivatePersonInvoice>> GetAllInvoices()
        {
            IQueryable<PrivatePersonInvoice> queryable = context.PrivatePersonInvoice.AsQueryable();

            var invoices = await queryable.ToListAsync();
            // foreach (var invoice in invoices)
            // {
            //     invoice.ProductsAndQuantities = invoice.ProductsAndQuantities;
            // }

            return invoices;
        }

        public async Task<PrivatePersonInvoice> SaveInvoiceInDb(PrivatePersonInvoice invoice)
        {
            // if (invoice.ProductsAndQuantities != null)
            // {
            //     invoice.ProductsAndQuantitiesJson = JsonConvert.SerializeObject(invoice.ProductsAndQuantities);
            // }

            context.Add(invoice);
            await context.SaveChangesAsync();
            return invoice;
        }

        public async Task<PrivatePersonInvoice?> GetInvoicesById(int id)
        {
            var invoice = await context.PrivatePersonInvoice.FindAsync(id);

            // if (invoice != null)
            // {
            //     invoice.ProductsAndQuantities = invoice.ProductsAndQuantities;
            // }

            return invoice;
        }

        public async Task<bool> InvoiceExistsInDb(int id)
        {
            return await context.PrivatePersonInvoice.AnyAsync(x => x.PrivatePersonInvoiceId == id);
        }

        public async Task<bool> DeleteInvoice(int id)
        {
            PrivatePersonInvoice? invoiceInDb = await GetInvoicesById(id);
            if (invoiceInDb == null)
            {
                return false;
            }
            else
            {
                context.Remove(invoiceInDb);
                int changesCount = await context.SaveChangesAsync();
                return changesCount == 1;
            }
        }

        // public async Task<List<Product>> GetProductsByIds(List<int> productIds)
        // {
        //     IQueryable<Product> query = context.Product.AsQueryable();
        //     query = query.Where(p => productIds.Contains(p.ProductId));
        //     return await query.ToListAsync();
        // }
    }
}