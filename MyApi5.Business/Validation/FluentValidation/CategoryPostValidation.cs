using FluentValidation;
using MyApi5.Entities.DTOs.CategoryDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi5.Business.Validation.FluentValidation
{
    public class CategoryPostValidation : AbstractValidator<CategoryPostDto>
    {
        public CategoryPostValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category should not be empty or null.")
                .MaximumLength(20).WithMessage("Kateqoriya adı 20 simvoldan uzun ola bilməz.")
                .MinimumLength(3).WithMessage("Kategoriya adi 3 simvoldan az olmamalidir.");
            RuleFor(x => x.FormFile).NotEmpty().WithMessage("Please add any photo for category.");
        }
    }
}
