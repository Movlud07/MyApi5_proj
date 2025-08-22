using FluentValidation;
using MyApi5.Entities.DTOs.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi5.Business.Validation.FluentValidation.UsersValidation
{
    public class UserRegisterValidator : AbstractValidator<UserRegisterDto>
    {
        public UserRegisterValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name field dont be empty.")
                .MaximumLength(20).WithMessage("Name field max length must be less than 20.");

            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Surname field dont be empty.")
                .MaximumLength(20).WithMessage("Surname field max length must be less than 25.");

            RuleFor(x => x.Email)
           .NotEmpty().WithMessage("Email boş ola bilməz.")
           .EmailAddress().WithMessage("Düzgün email formatı deyil.");

            RuleFor(x => x.Username)
               .NotEmpty().WithMessage("İstifadəçi adı boş ola bilməz.")
               .MaximumLength(20).WithMessage("İstifadəçi adı 20 simvoldan çox ola bilməz.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifrə boş ola bilməz.")
                .MinimumLength(6).WithMessage("Şifrə ən azı 6 simvol olmalıdır.")
                .MaximumLength(30).WithMessage("Şifrə 30 simvoldan çox ola bilməz.");

            RuleFor(x => x.Age)
                .InclusiveBetween(18, 100).WithMessage("Yaş 18 ilə 100 arasında olmalıdır.");
        }
    }
}
