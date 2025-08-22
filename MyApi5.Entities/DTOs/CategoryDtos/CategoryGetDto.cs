using Microsoft.AspNetCore.Http;
using MyApi5.Entities.concretes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi5.Entities.DTOs.CategoryDtos
{
    public class CategoryGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? ProductsCount { get; set; } 
        public List<string> ProductsName {  get; set; } = new List<string>();
        public string? ImageUrl{ get; set; }
    }
}
