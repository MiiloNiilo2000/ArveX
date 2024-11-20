using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;


namespace BackEnd.Models
{
    public class PrivatePersonInvoice
    {
        public int PrivatePersonInvoiceId { get; set; }
        public required string Name { get; set; }
        public required int InvoiceNumber { get; set; }
        public required DateTime DateCreated { get; set; }
        public required DateTime DateDue { get; set; }
        public required string Condition { get; set; }
        public required string DelayFine { get; set; }
        public required string Font { get; set; }

        // public string ProductsAndQuantitiesJson { get; set; }

        // [Newtonsoft.Json.JsonIgnore]
        // [NotMapped]
        // public Dictionary<int, int> ProductsAndQuantities
        // {
        //     get
        //     {
        //         return string.IsNullOrEmpty(ProductsAndQuantitiesJson)
        //             ? new Dictionary<int, int>()
        //             : JsonConvert.DeserializeObject<Dictionary<int, int>>(ProductsAndQuantitiesJson);
        //     }
        //     set
        //     {
        //         ProductsAndQuantitiesJson = JsonConvert.SerializeObject(value);
        //     }
        // }
    }
}