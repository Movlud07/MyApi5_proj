using Microsoft.AspNetCore.Http;
using MyApi5.Entities.DTOs.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi5.Entities.DTOs.CategoryDtos
{
    public class CategoryUpdateDto
    {
        public string? Name { get; set; }
        public IFormFile? FormFile { get; set; }
    }
}
