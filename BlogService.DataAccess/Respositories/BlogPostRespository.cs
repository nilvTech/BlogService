using BlogService.DataAccess.Models;
using BlogService.DataAccess.Respositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogService.DataAccess.Respositories
{
    public class BlogPostRespository : RepositoryBase<BlogPost>, IBlogPostRepository
    {

        private readonly BlogPostDbContext _dbContext;

        public BlogPostRespository(BlogPostDbContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<BlogPost>FindByIdAsync(int id)
        {
            return await _dbContext.BlogPosts.Include(p => p.Comments).FirstOrDefaultAsync(p => p.Id == id);
        }

    }
}
