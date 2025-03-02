using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Data;
using BackEnd.Models;
using BackEnd.QuestPDF_Helpers;
using BackEnd.Data.Repos;
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BackEnd.Extensions;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CreateInvoiceController : ControllerBase
    {
        private readonly CompanyInvoicesRepo _companyRepo;
        private readonly PrivatePersonInvoicesRepo _privatePersonRepo;
        private readonly UserManager<Profile> _userManager;
        protected decimal _totalPrice;
        protected double _taxPercent;
        protected decimal _priceWithoutTax;

        public CreateInvoiceController(CompanyInvoicesRepo companyRepo, PrivatePersonInvoicesRepo privatePersonRepo, UserManager<Profile> userManager)
        {
            _companyRepo = companyRepo;
            _privatePersonRepo = privatePersonRepo;
            _userManager = userManager;
        }

        [Authorize]
        [HttpPost("GeneratePdfcompany")]
        public async Task<IActionResult> GeneratePdf([FromBody] CompanyInvoice data)
        {
            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username);

            if (user == null){
                return Unauthorized("User not found.");
            }

            data.ProfileId = user.Id;

            var invoice = await _companyRepo.SaveInvoiceInDb(data);

            var products = await _companyRepo.GetProductsByIds(data.ProductsAndQuantities.Keys.ToList());

            return GeneratePdfResponse(invoice, products);
        }

        [Authorize]
        [HttpPost("GeneratePdfprivatePerson")]
        public async Task<IActionResult> GeneratePdf([FromBody] PrivatePersonInvoice data)
        {
            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username);

            if (user == null){
                return Unauthorized("User not found.");
            }

            data.ProfileId = user.Id;

            var invoice = await _privatePersonRepo.SaveInvoiceInDb(data);

            var products = await _privatePersonRepo.GetProductsByIds(data.ProductsAndQuantities.Keys.ToList());

            return GeneratePrivatePersonPdfResponse(invoice, products);
        }

        [HttpPost("GeneratePdfcompanyWithoutSaving")]
        public async Task<IActionResult> GeneratePdfWithoutSaving([FromBody] CompanyInvoice data)
        {
            var products = await _companyRepo.GetProductsByIds(data.ProductsAndQuantities.Keys.ToList());

            return GeneratePdfResponse(data, products);
        }

        [HttpPost("GeneratePdfprivatePersonWithoutSaving")]
        public async Task<IActionResult> GeneratePdfWithoutSaving([FromBody] PrivatePersonInvoice data)
        {
            var products = await _privatePersonRepo.GetProductsByIds(data.ProductsAndQuantities.Keys.ToList());

            return GeneratePrivatePersonPdfResponse(data, products);
        }

        private IActionResult GeneratePdfResponse(CompanyInvoice data, List<Product> products)
        {
            Console.WriteLine("Received Invoice Data: " + data);

            var document = CreateDocument(
                data.Title,
                data.ClientRegNr,
                data.ClientKMKR,
                data.Address,
                data.ZipCode,
                data.Country,
                data.InvoiceNumber,
                data.DateCreated,
                data.DateDue,
                data.Condition,
                data.DelayFine,
                data.Font,
                data.SenderCompanyName,
                data.SenderCompanyAddress,
                data.SenderCompanyRegistrationNumber,
                data.SenderCompanyKMKRNumber,
                products,
                data
            );

            var pdf = document.GeneratePdf();
            var sanitizedTitle = string.Join("_", data.Title.Split(Path.GetInvalidFileNameChars()));
            string fileName = $"{data.InvoiceNumber}";

            return File(pdf, "application/pdf", fileName);
        }

        private IActionResult GeneratePrivatePersonPdfResponse(PrivatePersonInvoice data, List<Product> products)
        {
            Console.WriteLine("Received Invoice Data: " + data);

            var document = CreatePrivatePersonDocument(
                data.Title,
                data.InvoiceNumber,
                data.DateCreated,
                data.DateDue,
                data.Condition,
                data.DelayFine,
                data.Font,
                data.SenderCompanyName,
                data.SenderCompanyAddress,
                data.SenderCompanyRegistrationNumber,
                data.SenderCompanyKMKRNumber,
                products,
                data
            );

            var pdf = document.GeneratePdf();
            var sanitizedTitle = string.Join("_", data.Title.Split(Path.GetInvalidFileNameChars()));
            string fileName = $"{data.InvoiceNumber}";

            return File(pdf, "application/pdf", fileName);
        }
        
        QuestPDF.Infrastructure.IDocument CreateDocument(
            string title,
            string clientRegNr,
            string clientKMKR,
            string address,
            string zipCode,
            string country,
            int invoiceNumber,
            DateTime dateCreated,
            DateTime dateDue,
            string condition,
            string delayFine,
            string font,
            string senderCompanyName,
            string senderCompanyAddress,
            int senderCompanyRegistrationNumber,
            string senderCompanyKMKRNumber,
            List<Product> products,
            CompanyInvoice data
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
                        .Column(col => 
                        {
                            col.Item().Table(headerTable =>
                            {
                                headerTable.ColumnsDefinition(columns => 
                                {
                                    columns.RelativeColumn();
                                    
                                });

                                headerTable.Cell().Row(1).Column(1).Table(innerTable =>
                                {
                                    innerTable.ColumnsDefinition(innerColumns =>
                                    {
                                        innerColumns.RelativeColumn(3);
                                        innerColumns.RelativeColumn(2);
                                    });

                                    innerTable.Cell().Column(1).Width(64).Image("assets/images/arvexLogo.png");

                                    innerTable.Cell().Column(2).Table(innerTableRight => 
                                    {
                                        innerTableRight.ColumnsDefinition(innerColumns =>
                                        {
                                            innerColumns.RelativeColumn();
                                            innerColumns.RelativeColumn();
                                        });
                                        innerTableRight.Cell().Row(1).Column(1).AlignLeft().Text("Arve number:").FontSize(15).Bold();
                                        innerTableRight.Cell().Row(1).Column(2).AlignRight().Text(invoiceNumber.ToString()).FontSize(15).Bold();
                                        innerTableRight.Cell().Row(2).Column(1).Padding(2);

                                        innerTableRight.Cell().Row(3).Column(1).AlignLeft().Text("Kuupäev:").FontSize(11);
                                        innerTableRight.Cell().Row(3).Column(2).AlignRight().Text(dateCreated.ToString("MM.dd.yyyy")).FontSize(11);
    
                                        innerTableRight.Cell().Row(4).Column(1).AlignLeft().Text("Maksetähtaeg:").FontSize(11).Bold();
                                        innerTableRight.Cell().Row(4).Column(2).AlignRight().Text(dateDue.ToString("MM.dd.yyyy")).FontSize(11).Bold();

                                        innerTableRight.Cell().Row(5).Column(1).AlignLeft().Text("Tingimused:").FontSize(11);
                                        innerTableRight.Cell().Row(5).Column(2).AlignRight().Text(condition).FontSize(11);

                                        innerTableRight.Cell().Row(6).Column(1).AlignLeft().Text("Viivis:").FontSize(11);
                                        innerTableRight.Cell().Row(6).Column(2).AlignRight().Text(delayFine).FontSize(11);
                                    });
                                });               
                            });
                        });

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(col =>
                        {
                            col.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(3);;
                                    columns.RelativeColumn(2);
                                });

                                table.Cell().Row(1).Column(1).Table(innerTable =>
                                {
                                    innerTable.ColumnsDefinition(innerColumns =>
                                    {
                                        innerColumns.RelativeColumn();
                                    });
                                   
                                    var parts = address.Split(',', 3); 

                                    string firstLine = parts.Length > 2 ? $"{parts[0]}, {parts[1]}" : address;
                                    string secondLine = parts.Length > 2 ? parts[2] : "";

                                    innerTable.Cell().Row(1).Column(1).AlignLeft().Text("Klient").FontSize(14);
                                    innerTable.Cell().Row(2).Column(1).Padding(2);
                                    innerTable.Cell().Row(3).Column(1).AlignLeft().Text(title).FontSize(16).Bold();
                                    innerTable.Cell().Row(4).Column(1).AlignLeft().Text("Reg. nr.: " + clientRegNr).FontSize(11);
                                    innerTable.Cell().Row(5).Column(1).AlignLeft().Text("KMKR: " + clientKMKR).FontSize(11);
                                    innerTable.Cell().Row(6).Column(1).AlignLeft().Text(secondLine.Trim()).FontSize(11);
                                    innerTable.Cell().Row(7).Column(1).AlignLeft().Text(firstLine).FontSize(11);
                                    innerTable.Cell().Row(8).Column(1).AlignLeft().Text(zipCode).FontSize(11);
                                    innerTable.Cell().Row(9).Column(1).AlignLeft().Text(country).FontSize(11);
                                });
                                    

                                table.Cell().Row(1).Column(2).Table(innerTable =>
                                {
                                    innerTable.ColumnsDefinition(innerColumns =>
                                    {
                                        innerColumns.RelativeColumn();
                                        innerColumns.RelativeColumn();
                                    });

                                    var parts = senderCompanyAddress.Split(',', 3); 

                                    string firstLine = parts.Length > 2 ? $"{parts[0]}, {parts[1]}" : senderCompanyAddress;
                                    string secondLine = parts.Length > 2 ? parts[2] : "";

                                    innerTable.Cell().Row(1).Column(1).AlignLeft().Text(senderCompanyName).FontSize(15).Bold();
                                    innerTable.Cell().Row(2).Column(1).Padding(2);
                                    
                                    innerTable.Cell().Row(3).Column(1).AlignLeft().Text(secondLine.Trim()).FontSize(11);
                                    innerTable.Cell().Row(4).Column(1).AlignLeft().Text(firstLine).FontSize(11);
                                    
                                    innerTable.Cell().Row(5).Column(1).AlignLeft().Text("Reg.Nr.: " + senderCompanyRegistrationNumber).FontSize(11);
                                    

                                    innerTable.Cell().Row(6).Column(1).AlignLeft().Text("KMKR: " + senderCompanyKMKRNumber).FontSize(11);

                                });
                            });

                            col.Item().PaddingTop(30).Table(productTable =>
                            {
                               
                                productTable.ColumnsDefinition(columns =>
                                {

                                    columns.RelativeColumn(3);
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(1);
                                });

                                productTable.Header(header =>
                                {
                                    header.Cell().Row(1).Column(1).Text("Toote Nimi").FontSize(16).Bold();
                                    header.Cell().Row(1).Column(3).Text("Hind(tk)").FontSize(16).Bold();
                                    header.Cell().Row(1).Column(2).Text("Kogus").FontSize(16).Bold();                              
                                    header.Cell().Row(1).Column(4).Text("Kokku").FontSize(16).Bold();
                                    header.Cell().Row(1).Column(5).Text("KM %").AlignRight().FontSize(16).Bold();
                                });           
                                productTable.Cell().PaddingTop(4);  
                                productTable.Cell().PaddingTop(4); 
                                productTable.Cell().PaddingTop(4);
                                productTable.Cell().PaddingTop(4);  
                                productTable.Cell().PaddingTop(4);
                                productTable.Cell().LineHorizontal(1);
                                productTable.Cell().LineHorizontal(1);
                                productTable.Cell().LineHorizontal(1);
                                productTable.Cell().LineHorizontal(1);
                                productTable.Cell().LineHorizontal(1);
                                productTable.Cell().Padding(5);
                                productTable.Cell().Padding(5);
                                productTable.Cell().Padding(5);
                                productTable.Cell().Padding(5);
                                productTable.Cell().Padding(5);

                                foreach (var product in products)
                                {
                                    int quantity = data.ProductsAndQuantities.ContainsKey(product.ProductId) 
                                        ? data.ProductsAndQuantities[product.ProductId] 
                                        : 1; 

                                    _taxPercent = product.TaxPercent;
                                    decimal taxPercentDecimal = (decimal)_taxPercent;
                                    decimal priceWithTax = product.Price * quantity + (product.Price * quantity * (taxPercentDecimal / 100));
                                    _totalPrice += priceWithTax;
                                    _priceWithoutTax += product.Price * quantity;

                                    productTable.Cell().Text(product.Name).FontSize(14);
                                    productTable.Cell().Text(quantity.ToString()).FontSize(14);
                                    productTable.Cell().Text(product.Price.ToString("C")).FontSize(14);
                                    productTable.Cell().Text(priceWithTax.ToString("C")).FontSize(14);
                                    productTable.Cell().AlignRight().Text(_taxPercent.ToString()).FontSize(14);
                                }
                            });

                            col.Item().PaddingVertical(10).LineHorizontal(1);
                            col.Item().Table(totalTable =>
                            {
                                totalTable.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn();
                                });
            
                                totalTable.Cell().AlignRight().Text("Summa").FontSize(14);
                                totalTable.Cell().AlignRight().Text(_priceWithoutTax.ToString("C")).FontSize(16);
                                totalTable.Cell().AlignRight().Text("+Käibemaks").FontSize(12);
                                totalTable.Cell().AlignRight().Text((_totalPrice - _priceWithoutTax).ToString("C")).FontSize(12);
                                totalTable.Cell().AlignRight().Text("Kokku").FontSize(16).Bold();
                                totalTable.Cell().AlignRight().Text(_totalPrice.ToString("C")).FontSize(16).Bold();
                            });
                        });

                    page.Footer()
                        .Row(row =>
                        {
                            row.RelativeItem().AlignLeft().Image("assets/images/TestFooter.png");
                        });
                });
            });
        }
        QuestPDF.Infrastructure.IDocument CreatePrivatePersonDocument(
            string title,
            int invoiceNumber,
            DateTime dateCreated,
            DateTime dateDue,
            string condition,
            string delayFine,
            string font,
            string senderCompanyName,
            string senderCompanyAddress,
            int senderCompanyRegistrationNumber,
            string senderCompanyKMKRNumber,
            List<Product> products,
            PrivatePersonInvoice data
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
                        .Column(col => 
                        {
                            col.Item().Table(headerTable =>
                            {
                                headerTable.ColumnsDefinition(columns => 
                                {
                                    columns.RelativeColumn();
                                    
                                });

                                headerTable.Cell().Row(1).Column(1).Table(innerTable =>
                                {
                                    innerTable.ColumnsDefinition(innerColumns =>
                                    {
                                        innerColumns.RelativeColumn(3);
                                        innerColumns.RelativeColumn(2);
                                    });

                                    innerTable.Cell().Column(1).Width(64).Image("assets/images/arvexLogo.png");

                                    innerTable.Cell().Column(2).Table(innerTableRight => 
                                    {
                                        innerTableRight.ColumnsDefinition(innerColumns =>
                                        {
                                            innerColumns.RelativeColumn();
                                            innerColumns.RelativeColumn();
                                        });
                                        innerTableRight.Cell().Row(1).Column(1).AlignLeft().Text("Arve number:").FontSize(15).Bold();
                                        innerTableRight.Cell().Row(1).Column(2).AlignRight().Text(invoiceNumber.ToString()).FontSize(15).Bold();
                                        innerTableRight.Cell().Row(2).Column(1).Padding(2);

                                        innerTableRight.Cell().Row(3).Column(1).AlignLeft().Text("Kuupäev:").FontSize(11);
                                        innerTableRight.Cell().Row(3).Column(2).AlignRight().Text(dateCreated.ToString("MM.dd.yyyy")).FontSize(11);
    
                                        innerTableRight.Cell().Row(4).Column(1).AlignLeft().Text("Maksetähtaeg:").FontSize(11).Bold();
                                        innerTableRight.Cell().Row(4).Column(2).AlignRight().Text(dateDue.ToString("MM.dd.yyyy")).FontSize(11).Bold();

                                        innerTableRight.Cell().Row(5).Column(1).AlignLeft().Text("Tingimused:").FontSize(11);
                                        innerTableRight.Cell().Row(5).Column(2).AlignRight().Text(condition).FontSize(11);

                                        innerTableRight.Cell().Row(6).Column(1).AlignLeft().Text("Viivis:").FontSize(11);
                                        innerTableRight.Cell().Row(6).Column(2).AlignRight().Text(delayFine).FontSize(11);
                                    });
                                });               
                            });
                        });

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(col =>
                        {
                            col.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(3);;
                                    columns.RelativeColumn(2);
                                });

                                table.Cell().Row(1).Column(1).Table(innerTable =>
                                {
                                    innerTable.ColumnsDefinition(innerColumns =>
                                    {
                                        innerColumns.RelativeColumn();
                                    });

                                    innerTable.Cell().Row(1).Column(1).AlignLeft().Text("Klient").FontSize(14);
                                    innerTable.Cell().Row(2).Column(1).Padding(2);
                                    innerTable.Cell().Row(3).Column(1).AlignLeft().Text(title).FontSize(16).Bold();
                                });
                                    

                                table.Cell().Row(1).Column(2).Table(innerTable =>
                                {
                                    innerTable.ColumnsDefinition(innerColumns =>
                                    {
                                        innerColumns.RelativeColumn();
                                        innerColumns.RelativeColumn();
                                    });

                                    var parts = senderCompanyAddress.Split(',', 3); 

                                    string firstLine = parts.Length > 2 ? $"{parts[0]}, {parts[1]}" : senderCompanyAddress;
                                    string secondLine = parts.Length > 2 ? parts[2] : "";

                                    innerTable.Cell().Row(1).Column(1).AlignLeft().Text(senderCompanyName).FontSize(15).Bold();
                                    innerTable.Cell().Row(2).Column(1).Padding(2);
                                    
                                    innerTable.Cell().Row(3).Column(1).AlignLeft().Text(secondLine.Trim()).FontSize(11);
                                    innerTable.Cell().Row(4).Column(1).AlignLeft().Text(firstLine).FontSize(11);
                                    
                                    innerTable.Cell().Row(5).Column(1).AlignLeft().Text("Reg.Nr.: " + senderCompanyRegistrationNumber).FontSize(11);
                                    

                                    innerTable.Cell().Row(6).Column(1).AlignLeft().Text("KMKR: " + senderCompanyKMKRNumber).FontSize(11);

                                });
                            });

                            col.Item().PaddingTop(30).Table(productTable =>
                            {
                               
                                productTable.ColumnsDefinition(columns =>
                                {

                                    columns.RelativeColumn(3);
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(1);
                                });

                                productTable.Header(header =>
                                {
                                    header.Cell().Row(1).Column(1).Text("Toote Nimi").FontSize(16).Bold();
                                    header.Cell().Row(1).Column(3).Text("Hind(tk)").FontSize(16).Bold();
                                    header.Cell().Row(1).Column(2).Text("Kogus").FontSize(16).Bold();                              
                                    header.Cell().Row(1).Column(4).Text("Kokku").FontSize(16).Bold();
                                    header.Cell().Row(1).Column(5).Text("KM %").AlignRight().FontSize(16).Bold();
                                });           
                                productTable.Cell().PaddingTop(4);  
                                productTable.Cell().PaddingTop(4); 
                                productTable.Cell().PaddingTop(4);
                                productTable.Cell().PaddingTop(4);  
                                productTable.Cell().PaddingTop(4);
                                productTable.Cell().LineHorizontal(1);
                                productTable.Cell().LineHorizontal(1);
                                productTable.Cell().LineHorizontal(1);
                                productTable.Cell().LineHorizontal(1);
                                productTable.Cell().LineHorizontal(1);
                                productTable.Cell().Padding(5);
                                productTable.Cell().Padding(5);
                                productTable.Cell().Padding(5);
                                productTable.Cell().Padding(5);
                                productTable.Cell().Padding(5);

                                foreach (var product in products)
                                {
                                    int quantity = data.ProductsAndQuantities.ContainsKey(product.ProductId) 
                                        ? data.ProductsAndQuantities[product.ProductId] 
                                        : 1; 

                                    _taxPercent = product.TaxPercent;
                                    decimal taxPercentDecimal = (decimal)_taxPercent;
                                    decimal priceWithTax = product.Price * quantity + (product.Price * quantity * (taxPercentDecimal / 100));
                                    _totalPrice += priceWithTax;
                                    _priceWithoutTax += product.Price * quantity;

                                    productTable.Cell().Text(product.Name).FontSize(14);
                                    productTable.Cell().Text(quantity.ToString()).FontSize(14);
                                    productTable.Cell().Text(product.Price.ToString("C")).FontSize(14);
                                    productTable.Cell().Text(priceWithTax.ToString("C")).FontSize(14);
                                    productTable.Cell().AlignRight().Text(_taxPercent.ToString()).FontSize(14);
                                }
                            });

                            col.Item().PaddingVertical(10).LineHorizontal(1);
                            col.Item().Table(totalTable =>
                            {
                                totalTable.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn();
                                });
            
                                totalTable.Cell().AlignRight().Text("Summa").FontSize(14);
                                totalTable.Cell().AlignRight().Text(_priceWithoutTax.ToString("C")).FontSize(16);
                                totalTable.Cell().AlignRight().Text("+Käibemaks").FontSize(12);
                                totalTable.Cell().AlignRight().Text((_totalPrice - _priceWithoutTax).ToString("C")).FontSize(12);
                                totalTable.Cell().AlignRight().Text("Kokku").FontSize(16).Bold();
                                totalTable.Cell().AlignRight().Text(_totalPrice.ToString("C")).FontSize(16).Bold();
                            });
                        });

                    page.Footer()
                        .Row(row =>
                        {
                            row.RelativeItem().AlignLeft().Image("assets/images/TestFooter.png");
                        });
                });
            });
        }
    }
}