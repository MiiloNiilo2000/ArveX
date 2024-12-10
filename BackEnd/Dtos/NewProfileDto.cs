using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Dtos
{
    public class NewProfileDto
    {
        public required string UserName {get; set;}
        public required string Email {get; set;}
        public required string Token {get; set;}
    }
}