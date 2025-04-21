using BlogService.Core.Services.Interfaces;
using BlogService.DataAccess.DTOs;
using BlogService.DataAccess.Models;
using BlogService.DataAccess.Respositories.Interfaces;
using System.Linq.Expressions;

namespace BlogService.Core.Services
{
    public class BlogPostService : IBlogPostService
    {
        private readonly IBlogPostRepository _blogPostRepository;

        public BlogPostService(IBlogPostRepository blogPostRepository)
        {
            _blogPostRepository = blogPostRepository;
        }

        public async Task<BlogPost> CreatePostAsync(BlogPostDTO post)
        {
            var newPost = new BlogPost
            {
                Title = post.Title,
                Content = post.Content,
                PublishedDate = DateTime.UtcNow,
                LastUpdatedDate = DateTime.UtcNow,
                Comments = new List<Comment>()
            };

            return await _blogPostRepository.CreateAsync(newPost);
        }

        public Task<BlogPost> GetPostByIdAsync(int postId) =>
            _blogPostRepository.FindByIdAsync(postId);

        public Task DeletePostAsync(int id) =>
            _blogPostRepository.DeletePostAsync(id);

        public Task<IEnumerable<BlogPost>> GetAllBlogsAsync(
            Expression<Func<BlogPost, bool>>? filter = null,
            string? includeProperties = null) =>
            _blogPostRepository.GetAllAsync(includeProperties: nameof(BlogPost.Comments));
    }
}
