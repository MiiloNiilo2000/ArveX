using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BackEnd.Models
{
    public class Company
    {
    public int  CompanyId { get; set; }
    public string? Name { get; set; }
    public int RegisterCode { get; set; }
    public string? VATnumber { get; set; }
    public string? Address { get; set; }
    public int PostalCode { get; set; }
    public string? Country { get; set; }
    public string? Email { get; set; }
    public string? Image { get; set; }
    [JsonIgnore]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}