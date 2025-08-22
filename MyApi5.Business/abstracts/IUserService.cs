using MyApi5.Entities.concretes;
using MyApi5.Entities.DTOs.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi5.Business.abstracts
{
    public interface IUserService
    {
        Task Register(UserRegisterDto registerDto);
        Task<string> Login(UserLoginDto loginDto);
    }
}
