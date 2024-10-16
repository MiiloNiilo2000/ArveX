using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Companion;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;


namespace BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CreateInvoiceController : ControllerBase
    {
        [HttpPost(Name = "GeneratePdf")]
        public IResult GeneratePdf([FromBody] InvoiceData data)
        {
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
                data.Font
            );
            
            var pdf = document.GeneratePdf();
            // document.ShowInCompanion();
            
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
            string font
            )
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(40);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x =>
                    {
                        return x
                        .FontSize(20)
                        .FontFamily(font);
                    });

                    page.Header()
                        .Text(title)
                        .SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Table(table => {
                        table.ColumnsDefinition(columns =>
                        {
                            
                            columns.RelativeColumn();
                            columns.RelativeColumn();

                        });
                        table.Cell().Row(1).Column(x => {
                            x.Spacing(2);
                            x.Item().Text("Klient: ").FontSize(15);
                            x.Item().Padding(2);
                            x.Item().Text(title).Bold().FontSize(18);
                            x.Item().Text(address).FontSize(15);
                            x.Item().Text(zipCode).FontSize(15);
                            x.Item().Text(country).FontSize(15);
                        });
                        table.Cell().Row(1).Column(2).Column(x =>
                        {
                            x.Item().Table(innerTable =>
                            {
                                innerTable.ColumnsDefinition(innerColumns =>
                                {
                                    innerColumns.RelativeColumn(); 
                                    innerColumns.RelativeColumn(); 
                                });
                                innerTable.Cell().Row(0).ColumnSpan(2).Height(25);

                                innerTable.Cell().Row(1).Column(1).AlignLeft().Text("Arve number:").FontSize(15).Bold();
                                innerTable.Cell().Row(1).Column(2).AlignRight().Text(invoiceNumber.ToString()).FontSize(15).Bold();

                                innerTable.Cell().Row(2).Column(1).AlignLeft().Text("Kuupäev:").FontSize(12);
                                innerTable.Cell().Row(2).Column(2).AlignRight().Text(dateCreated.ToString("MM.dd.yyyy")).FontSize(12);

                                innerTable.Cell().Row(3).Column(1).AlignLeft().Text("Tingimused:").FontSize(12);
                                innerTable.Cell().Row(3).Column(2).AlignRight().Text(condition).FontSize(12);

                                innerTable.Cell().Row(4).Column(1).AlignLeft().Text("Maksetähtaeg:").FontSize(12);
                                innerTable.Cell().Row(4).Column(2).AlignRight().Text(dateDue.ToString("MM.dd.yyyy")).FontSize(12);

                                innerTable.Cell().Row(5).Column(1).AlignLeft().Text("Viivis:").FontSize(12);
                                innerTable.Cell().Row(5).Column(2).AlignRight().Text(delayFine).FontSize(12);
                            });
                        });
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ").FontSize(15).FontFamily("Times New Roman");
                            x.CurrentPageNumber().FontSize(15).FontFamily("Times New Roman");
                        });
                });
            });
        }
    }
}