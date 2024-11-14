using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackEnd.Models
{
    public class Profile : IdentityUser
    { 
    public string? Image {get; set; }
    [JsonIgnore]
    public virtual ICollection<Company> Companies { get; set; } = new List<Company>();   
    }
}