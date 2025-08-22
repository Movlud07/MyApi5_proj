using FluentValidation;
using MyApi5.Entities.DTOs.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi5.Business.Validation.FluentValidation.UsersValidation
{
    public class UserLoginValidator : AbstractValidator<UserLoginDto>
    {
        public UserLoginValidator()
        {

            RuleFor(x => x.Username)
               .NotEmpty().WithMessage("İstifadəçi adı boş ola bilməz.")
               .MaximumLength(20).WithMessage("İstifadəçi adı 20 simvoldan çox ola bilməz.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifrə boş ola bilməz.")
                .MinimumLength(6).WithMessage("Şifrə ən azı 6 simvol olmalıdır.")
                .MaximumLength(30).WithMessage("Şifrə 30 simvoldan çox ola bilməz.");
        }
    }
}
