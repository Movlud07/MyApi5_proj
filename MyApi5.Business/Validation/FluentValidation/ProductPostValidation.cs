using FluentValidation;
using MyApi5.Entities.concretes;
using MyApi5.Entities.DTOs.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi5.Business.Validation.FluentValidation
{
    public class ProductPostValidation : AbstractValidator<ProductPostDto>
    {
        public ProductPostValidation()
        {
            RuleFor(x => x.Quantity).NotEmpty().WithMessage("Id must be dolu").GreaterThanOrEqualTo(0);
            RuleFor(x => x.Name).NotEmpty().WithMessage("Product Name is required").MinimumLength(2).WithMessage("Product name must be greater than 2 letter");
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Product need CategoryId be patient").GreaterThanOrEqualTo(1);
        }
    }
}
