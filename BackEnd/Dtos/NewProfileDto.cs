using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Dtos
{
    public class NewProfileDto
    {
        public string UserName {get; set;}
        public string Email {get; set;}
        public string Token {get; set;}
    }
}