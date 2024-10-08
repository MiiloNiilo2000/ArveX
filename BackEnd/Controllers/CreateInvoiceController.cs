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
            var document = CreateDocument(data.Title);
            
            var pdf = document.GeneratePdf();
            document.ShowInCompanion();

            return Results.File(pdf, "application/pdf", "invoice.pdf");
        }

        QuestPDF.Infrastructure.IDocument CreateDocument(string title)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(20));

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
                            x.Item().Text("Ehitajate tee 5").FontSize(15);
                            x.Item().Text("12345, Tallinn").FontSize(15);
                            x.Item().Text("Eesti").FontSize(15);
                        });
                        table.Cell().Row(1).Column(2).Text("B");

                    });
                        
                        //     x.Item()
                        //     .Height(100)
                        //     .Width(400)
                        //     .Image("assets/images/pahandus.jpg");

                        // });


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