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
    public class InvoicesRepoBase<T> where T : class
    {
        protected readonly ApplicationDbContext context;

        public InvoicesRepoBase(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<List<T>> GetAllInvoices()
        {
            return await context.Set<T>().ToListAsync();
        }

        public async Task<T> SaveInvoiceInDb(T invoice)
        {
            context.Add(invoice);
            await context.SaveChangesAsync();
            return invoice;
        }

        public async Task<T?> GetInvoicesById(int id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public async Task<bool> InvoiceExistsInDb(int id)
        {
            return await context.Set<T>().AnyAsync(x => EF.Property<int>(x, "InvoiceId") == id);
        }

        public async Task<bool> DeleteInvoice(int id)
        {
            var invoice = await GetInvoicesById(id);
            if (invoice == null) return false;

            context.Remove(invoice);
            int changesCount = await context.SaveChangesAsync();
            return changesCount == 1;
        }
    }
}