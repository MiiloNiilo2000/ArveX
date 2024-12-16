using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Dtos
{
    public class UpdatePasswordDto
    {
        public required string OldPassword {get; set;}
        public required string NewPassword {get; set;}
    }
}