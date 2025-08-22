using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi5.Entities.DTOs.UserDtos
{
    public class UserLoginDto
    {
        public string? Username {  get; set; }
        public string? Password { get; set; }
    }
}
