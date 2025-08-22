using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi5.Entities.DTOs.ProductDtos
{
    public class ProductUpdateDto
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int? CategoryId { get; set; }
    }
}
