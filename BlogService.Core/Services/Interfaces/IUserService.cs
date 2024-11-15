using BlogService.DataAccess.DTOs;
using BlogService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogService.Core.Services.Interfaces
{
    public interface IUserService
    {
        Task<string> Login(string userName, string password);
        Task<User> Register(User user);
    }
}
