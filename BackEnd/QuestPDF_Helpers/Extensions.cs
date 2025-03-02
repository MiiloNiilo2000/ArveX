using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuestPDF.Companion;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;

namespace BackEnd.QuestPDF_Helpers
{
    public static class Extensions
    {
         private static IContainer Cell(this IContainer container, bool dark)
        {
            return container
                .Border(1)
                .Background(dark ? Colors.Grey.Lighten2 : Colors.White)
                .Padding(10);
        }
        
        // displays only text label
        public static void LabelCell(this IContainer container, string text) => container.Cell(true).Text(text).Medium();
        
        // allows you to inject any type of content, e.g. image
        public static IContainer ValueCell(this IContainer container) => container.Cell(false);
    }
}