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

        public async Task<List<CompanyInvoice>> GetAllInvoices()
        {
            IQueryable<CompanyInvoice> queryable = context.CompanyInvoice.AsQueryable();

            var invoices = await queryable.ToListAsync();
            // foreach (var invoice in invoices)
            // {
            //     invoice.ProductsAndQuantities = invoice.ProductsAndQuantities;
            // }

            return invoices;
        }

        public async Task<CompanyInvoice> SaveInvoiceInDb(CompanyInvoice invoice)
        {
            // if (invoice.ProductsAndQuantities != null)
            // {
            //     invoice.ProductsAndQuantitiesJson = JsonConvert.SerializeObject(invoice.ProductsAndQuantities);
            // }

            context.Add(invoice);
            await context.SaveChangesAsync();
            return invoice;
        }

        public async Task<CompanyInvoice?> GetInvoicesById(int id)
        {
            var invoice = await context.CompanyInvoice.FindAsync(id);

            // if (invoice != null)
            // {
            //     invoice.ProductsAndQuantities = invoice.ProductsAndQuantities;
            // }

            return invoice;
        }

        public async Task<bool> InvoiceExistsInDb(int id)
        {
            return await context.CompanyInvoice.AnyAsync(x => x.CompanyInvoiceId == id);
        }

        public async Task<bool> DeleteInvoice(int id)
        {
            CompanyInvoice? invoiceInDb = await GetInvoicesById(id);
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