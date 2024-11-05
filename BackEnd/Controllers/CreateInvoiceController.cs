using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Data;
using BackEnd.Models;
using BackEnd.QuestPDF_Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Companion;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CreateInvoiceController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        protected double _totalPrice;
        protected double _taxPercent;
        protected double _priceWithoutTax;
        public CreateInvoiceController(ApplicationDbContext context){
            _context = context;
        }

        [HttpPost(Name = "GeneratePdf")]
        public async Task<IResult> GeneratePdf([FromBody] Invoice data)
        {
            Console.WriteLine("Received Invoice Data: " + data);

            _context.Invoice.Add(data);
            await _context.SaveChangesAsync();

            var products = await _context.Product
                .Where(p => data.ProductIds.Contains(p.ProductId))
                .ToListAsync();

            var document = CreateDocument(
                data.Title, 
                data.Address, 
                data.ZipCode, 
                data.Country, 
                data.InvoiceNumber, 
                data.DateCreated, 
                data.DateDue, 
                data.Condition, 
                data.DelayFine,
                data.Font,
                products
            );
            
            var pdf = document.GeneratePdf();
            document.ShowInCompanion();
            var sanitizedTitle = string.Join("_", data.Title.Split(Path.GetInvalidFileNameChars()));
           
            string fileName = $"{sanitizedTitle}_invoice_{data.InvoiceNumber}";
                 
            return Results.File(pdf, "application/pdf", fileName);
        }
        
        QuestPDF.Infrastructure.IDocument CreateDocument(
            string title,
            string address,
            string zipCode,
            string country,
            int invoiceNumber,
            DateTime dateCreated,
            DateTime dateDue,
            string condition,
            string delayFine,
            string font,
            List<Product> products
            )
        {
           return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(40);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(20).FontFamily(font));

                    page.Header()
                        .Text(title)
                        .SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(col =>
                        {
                            col.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });

                                table.Cell().Row(1).Column(1).Table(innerTable =>
                                {
                                    innerTable.ColumnsDefinition(innerColumns =>
                                    {
                                        innerColumns.RelativeColumn();
                                    });
                                    innerTable.Cell().Row(1).Column(1).AlignLeft().Text("Klient:").FontSize(15);
                                    innerTable.Cell().Row(2).Column(1).Padding(2);
                                    innerTable.Cell().Row(3).Column(1).AlignLeft().Text(title).FontSize(18).Bold();
                                    innerTable.Cell().Row(4).Column(1).AlignLeft().Text(address).FontSize(12);
                                    innerTable.Cell().Row(5).Column(1).AlignLeft().Text(zipCode).FontSize(12);
                                    innerTable.Cell().Row(6).Column(1).AlignLeft().Text(country).FontSize(12);
                                });
                                    

                                table.Cell().Row(1).Column(2).Table(innerTable =>
                                {
                                    innerTable.ColumnsDefinition(innerColumns =>
                                    {
                                        innerColumns.RelativeColumn();
                                        innerColumns.RelativeColumn();
                                    });

                                    innerTable.Cell().Row(1).Column(1).AlignLeft().Text("Arve number:").FontSize(15).Bold();
                                    innerTable.Cell().Row(1).Column(2).AlignRight().Text(invoiceNumber.ToString()).FontSize(15).Bold();
                                    innerTable.Cell().Row(2).Column(1).Padding(2);
                                    
                                    innerTable.Cell().Row(3).Column(1).AlignLeft().Text("Kuupäev:").FontSize(12);
                                    innerTable.Cell().Row(3).Column(2).AlignRight().Text(dateCreated.ToString("MM.dd.yyyy")).FontSize(12);

                                    innerTable.Cell().Row(4).Column(1).AlignLeft().Text("Tingimused:").FontSize(12);
                                    innerTable.Cell().Row(4).Column(2).AlignRight().Text(condition).FontSize(12);

                                    innerTable.Cell().Row(5).Column(1).AlignLeft().Text("Maksetähtaeg:").FontSize(12);
                                    innerTable.Cell().Row(5).Column(2).AlignRight().Text(dateDue.ToString("MM.dd.yyyy")).FontSize(12);

                                    innerTable.Cell().Row(6).Column(1).AlignLeft().Text("Viivis:").FontSize(12);
                                    innerTable.Cell().Row(6).Column(2).AlignRight().Text(delayFine).FontSize(12);
                                });
                            });
                            col.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Black);
                            col.Item().PaddingTop(100).Table(productTable =>
                            {

                                // productTable.ColumnsDefinition(columns =>
                                // {
                                //     columns.ConstantColumn(100);
                                //     columns.ConstantColumn(100);
                                //     columns.RelativeColumn(2);
                                //     columns.RelativeColumn(3);
                                // });

                                // productTable.Cell().ValueCell().Text("test").FontSize(14);

                               
                                productTable.ColumnsDefinition(columns =>
                                {

                                    columns.RelativeColumn(3);
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(1);
                                });

                                productTable.Header(header =>
                                {
                                    header.Cell().Row(1).Column(1).Text("Toote Nimi").FontSize(16).Bold();
                                    header.Cell().Row(1).Column(2).Text("Hind").FontSize(16).Bold();
                                    header.Cell().Row(1).Column(3).Text("Kokku").FontSize(16).Bold();
                                    header.Cell().Row(1).Column(4).Text("KM %").FontSize(16).Bold();
                                });           
                                productTable.Cell().PaddingTop(4);  
                                productTable.Cell().PaddingTop(4); 
                                productTable.Cell().PaddingTop(4);
                                productTable.Cell().PaddingTop(4);
                                productTable.Cell().LineHorizontal(1);
                                productTable.Cell().LineHorizontal(1);
                                productTable.Cell().LineHorizontal(1);
                                productTable.Cell().LineHorizontal(1);
                                productTable.Cell().Padding(5);
                                productTable.Cell().Padding(5);
                                productTable.Cell().Padding(5);
                                productTable.Cell().Padding(5);

                                int rowNumber = 2;
                                foreach (var product in products)
                                {   
                                    _taxPercent = product.TaxPercent;
                                    double priceWithTax = product.Price + (product.Price * (_taxPercent/100));
                                    _totalPrice += priceWithTax;
                                    _priceWithoutTax += product.Price;
                                    productTable.Cell().Text(product.Name).FontSize(14);
                                    productTable.Cell().Text(product.Price.ToString("C")).FontSize(14);
                                    productTable.Cell().Text(priceWithTax.ToString("C")).FontSize(14);
                                    productTable.Cell().Text(_taxPercent).FontSize(14);
                                }
                            });
                            col.Item().PaddingVertical(10).LineHorizontal(1);
                            col.Item().Table(totalTable =>
                            {
                                totalTable.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(3);
                                    columns.RelativeColumn(1);
                                });
            
                                totalTable.Cell().Text("Summa:").FontSize(14);
                                totalTable.Cell().Text(_priceWithoutTax.ToString("C")).FontSize(16);
                                totalTable.Cell().Text("+Käibemaks").FontSize(12);
                                totalTable.Cell().Text((_totalPrice - _priceWithoutTax).ToString("C")).FontSize(12);
                                totalTable.Cell().Text("Kokku").FontSize(16).Bold();
                                totalTable.Cell().Text(_totalPrice.ToString("C")).FontSize(16).Bold();
                            });
                        });

                    page.Footer()
                        .Row(row =>
                        {
                            row.RelativeItem().AlignLeft().Image("assets/images/TestFooter.png", ImageScaling.FitWidth);
                        });
                });
            });
        }
    }
}