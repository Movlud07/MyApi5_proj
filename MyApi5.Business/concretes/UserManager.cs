using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyApi5.Business.abstracts;
using MyApi5.Business.Errors;
using MyApi5.Entities.concretes;
using MyApi5.Entities.DTOs.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Collections;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;


namespace MyApi5.Business.concretes
{
    public class UserManager : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly TokenOption _tokenOptions;
        public UserManager(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _configuration = configuration;
            _tokenOptions = _configuration.GetSection("TokenOptions").Get<TokenOption>();
        }
        public async Task<string> Login(UserLoginDto loginDto)
        {
            var dbUser = await _userManager.FindByNameAsync(loginDto.Username);
            if (dbUser is null)
            {
                //throw new ElementNotExistException("This user not found.");
                throw new RestException(StatusCodes.Status404NotFound, "This user not found.");
            }
            ;
            if (!await _userManager.CheckPasswordAsync(dbUser, loginDto.Password))
            {
                //throw new PasswordStateException("Password is incorrect.");
                throw new RestException(StatusCodes.Status406NotAcceptable,"Password", "Password is incorrect");
            }
            ICollection<string> roles = await _userManager.GetRolesAsync(dbUser);

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, dbUser.Id));
            claims.Add(new Claim(ClaimTypes.Name, dbUser.UserName));
            claims.Add(new Claim(ClaimTypes.Email, dbUser.Email));
            claims.Add(new Claim("FullName",dbUser.FullName));
            claims.Add(new Claim("Age", dbUser.Age.ToString()));
            claims.Add(new Claim("Name", dbUser.Name));
            claims.Add(new Claim("Surname", dbUser.Surname));
            claims.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x)));

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.SecurityKey));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            JwtHeader jwtHeader = new JwtHeader(credentials);
            JwtPayload jwtPayload = new JwtPayload(_tokenOptions.Issuer,_tokenOptions.Audience,claims,DateTime.UtcNow,DateTime.UtcNow.AddMinutes(_tokenOptions.AccessTokenExpirationTime));
            JwtSecurityToken jwtToken = new JwtSecurityToken(jwtHeader,jwtPayload);
            string token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return token;
        }

        public async Task Register(UserRegisterDto registerDto)
        {
            AppUser? existingUser = await _userManager.FindByNameAsync(registerDto.Username);
            if (existingUser != null)
            {
                throw new RestException(StatusCodes.Status409Conflict,"This username is exists, please take another.");
            }
            AppUser? dbUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (dbUser != null)
            {
                throw new RestException(StatusCodes.Status409Conflict,"This email exist in our database.");
            }
            AppUser user = _mapper.Map<AppUser>(registerDto);
            user.FullName = registerDto.Name + " " + registerDto.Surname;

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    throw new RestException(StatusCodes.Status409Conflict,item.Description);
                }
            }

            await _userManager.AddToRoleAsync(user, "User");

        }

    }
}
