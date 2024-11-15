using BlogService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogService.DataAccess.Respositories.Interfaces
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<User> FindByUserNameAndPasswordAsync(string userName, string passsword);
    }
}
