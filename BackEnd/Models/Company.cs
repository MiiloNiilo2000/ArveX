using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models
{
    public class Company
    {
    public int  Id { get; set; }
    public string? Name { get; set; }
    public int RegisterCode { get; set; }
    public string? VATnumber { get; set; }
    public string? Address { get; set; }
    public int PostalCode { get; set; }
    public string? Country { get; set; }
    public string? Email { get; set; }
    public string? Image { get; set; }
    }
}