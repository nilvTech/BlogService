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

            var createdPost = await _blogPostRepository.CreateAsync(newPost);
            return createdPost;
        }

        public async Task<BlogPost> GetPostByIdAsync(int postId)
        {
            return await _blogPostRepository.FindByIdAsync(postId);
        }

        public async Task<IEnumerable<BlogPost>> GetAllBlogsAsync(Expression<Func<BlogPost, bool>>? filter = null, string? includeProperties = null)
        {
            return await _blogPostRepository.GetAllAsync(includeProperties : nameof(BlogPost.Comments));
        }


    }
}
