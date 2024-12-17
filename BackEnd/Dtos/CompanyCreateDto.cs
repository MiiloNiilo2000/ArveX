using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Dtos
{
    public class CompanyCreateDto
    {
        public string Name { get; set; } = null!;
        public int RegisterCode { get; set; }
        public string VatNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public int PostalCode { get; set; }
        public string Country { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Bank { get; set; } 
        public string? IBAN { get; set; } 
        public string? SWIFT { get; set; } 
        public string? Image { get; set; }

    }
}