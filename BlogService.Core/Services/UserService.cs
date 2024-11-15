using BlogService.Core.Services.Interfaces;
using BlogService.DataAccess.DTOs;
using BlogService.DataAccess.Respositories.Interfaces;
using BlogService.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlogService.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;

        public UserService(IConfiguration configuration, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _config = configuration;
        }

        public async Task<string> Login(string userName, string password)
        {
            var user = await GetUser(userName, password);

            if (user == null)
            {
                return string.Empty;
            }

            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, user.Role)
                };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            var signIn = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _config["JWT:Issuer"],
                _config["JWT:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: signIn);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<User> Register(User user)
        {
            var users = await _userRepository.GetAllAsync(u => u.UserName == user.UserName);
            if (users.Any())
            {
                throw new InvalidOperationException("Username is already in use.");
            }
            using var hmac = new HMACSHA512();

            var registerUser = new User
            {
                UserName = user.UserName,
                Role = user.Role,
                Password = user.Password,  // hmac.ComputeHash(Encoding.UTF8.GetBytes(password)
                // PasswordSalt = hmac.Key
                Token= user.Token
            };

            var registeredUser = await _userRepository.CreateAsync(registerUser);

            return registeredUser;
        }

        private async Task<User> GetUser(string userName, string password)
        {
            return await _userRepository.FindByUserNameAndPasswordAsync(userName, password);
        }

    }
}
