using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BackEnd.Models
{
    public class Company
    {
    public int CompanyId { get; set; }
    public string Name { get; set; } = null!;
    public int RegisterCode { get; set; }
    public string VatNumber { get; set; } = null!;
    public string Address { get; set; } = null!;
    public int PostalCode { get; set; }
    public string Country { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Image { get; set; }
    public string ProfileId { get; set; } = null!;
    [JsonIgnore]
    public Profile profile { get; set; } = null!;
    [JsonIgnore]
    public ICollection<Product> Products { get; set; } = null!;
    }
}