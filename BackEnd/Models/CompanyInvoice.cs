using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace BackEnd.Models
{
    public class CompanyInvoice : PrivatePersonInvoice
    {
        public int CompanyInvoiceId { get; set; }
        public required string Address { get; set; }
        public required string ZipCode { get; set; }
        public required string Country { get; set; }
        public required string ClientRegNr { get; set; }
        public required string ClientKMKR { get; set; }

    }
}