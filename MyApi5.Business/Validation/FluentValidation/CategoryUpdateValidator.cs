using FluentValidation;
using MyApi5.Entities.DTOs.CategoryDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi5.Business.Validation.FluentValidation
{
    public class CategoryUpdateValidator : AbstractValidator<CategoryPostDto>
    {
        public CategoryUpdateValidator()
        {
            RuleFor(x => x.Name)
               .NotEmpty().WithMessage("Category should not be empty or null.")
               .MaximumLength(100).WithMessage("Kateqoriya adı 100 simvoldan uzun ola bilməz.")
               .MinimumLength(3).WithMessage("Kategoriya adi 3 simvoldan az olmamalidir.");
        }
    }
}
