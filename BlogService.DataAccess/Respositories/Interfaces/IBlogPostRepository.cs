using BlogService.DataAccess.Models;

namespace BlogService.DataAccess.Respositories.Interfaces
{
    public interface IBlogPostRepository : IRepositoryBase<BlogPost>
    {
        Task<BlogPost> FindByIdAsync(int id);

        Task<BlogPost> DeletePostAsync(int id);
    }
}
