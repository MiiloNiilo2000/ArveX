using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BackEnd.Models;
public class Product
{
    [Key] public int ProductId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int Price { get; set; }
    public int? CompanyId { get; set; }
    [JsonIgnore]
    public Company? company { get; set; } = null!;
    [JsonIgnore]
    public Profile? profile {get; set;} = null!;
    public double TaxPercent { get; set; }

}