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
    public class InvoiceRepo
    {
        private readonly ApplicationDbContext context;

        public InvoiceRepo(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Invoice>> GetAllInvoices()
        {
            IQueryable<Invoice> queryable = context.Invoice.AsQueryable();

            // Ensure ProductsAndQuantities is deserialized
            var invoices = await queryable.ToListAsync();
            foreach (var invoice in invoices)
            {
                invoice.ProductsAndQuantities = invoice.ProductsAndQuantities;
            }

            return invoices;
        }

        public async Task<Invoice> SaveInvoiceInDb(Invoice invoice)
        {
            // Ensure ProductsAndQuantities is serialized before saving
            if (invoice.ProductsAndQuantities != null)
            {
                // Set the ProductsAndQuantitiesJson before saving
                invoice.ProductsAndQuantitiesJson = JsonConvert.SerializeObject(invoice.ProductsAndQuantities);
            }

            context.Add(invoice);
            await context.SaveChangesAsync();
            return invoice;
        }

        public async Task<Invoice?> GetInvoicesById(int id)
        {
            var invoice = await context.Invoice.FindAsync(id);

            // Ensure ProductsAndQuantities is deserialized
            if (invoice != null)
            {
                invoice.ProductsAndQuantities = invoice.ProductsAndQuantities;
            }

            return invoice;
        }

        public async Task<bool> InvoiceExistsInDb(int id)
        {
            return await context.Invoice.AnyAsync(x => x.InvoiceId == id);
        }

        public async Task<bool> DeleteInvoice(int id)
        {
            Invoice? invoiceInDb = await GetInvoicesById(id);
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

        public async Task<List<Product>> GetProductsByIds(List<int> productIds)
        {
            IQueryable<Product> query = context.Product.AsQueryable();
            query = query.Where(p => productIds.Contains(p.ProductId));
            return await query.ToListAsync();
        }
    }
}