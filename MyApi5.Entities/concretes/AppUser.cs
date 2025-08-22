using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi5.Entities.concretes
{
    public class AppUser : IdentityUser
    {
        public string? FullName { get; set; }
        public int Age { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
    }
}
