using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BackEnd.Models
{
    public class Profile
    {
    public int ProfileId { get; set; }
    public string Username {get; set; } = null!;   
    public string Email {get; set; } = null!;
    public string? Image {get; set; }
    [JsonIgnore]
    public virtual ICollection<Company> Companies { get; set; } = new List<Company>();
    }
}