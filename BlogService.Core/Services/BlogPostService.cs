using System.Linq.Expressions;
using BlogService.Core.Services.Interfaces;
using BlogService.DataAccess.DTOs;
using BlogService.DataAccess.Models;
using BlogService.DataAccess.Respositories.Interfaces;

namespace BlogService.Core.Services
{
    public sealed class BlogPostService : IBlogPostService
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private const string DefaultIncludes = nameof(BlogPost.Comments);

        public BlogPostService(IBlogPostRepository blogPostRepository)
            => _blogPostRepository = blogPostRepository ?? throw new ArgumentNullException(nameof(blogPostRepository));

        public async Task<BlogPost> CreatePostAsync(BlogPostDTO post, CancellationToken ct = default)
        {
            if (post is null) throw new ArgumentNullException(nameof(post));
            if (string.IsNullOrWhiteSpace(post.Title)) throw new ArgumentException("Title is required.", nameof(post.Title));
            if (string.IsNullOrWhiteSpace(post.Content)) throw new ArgumentException("Content is required.", nameof(post.Content));

            var now = DateTime.UtcNow;

            var newPost = new BlogPost
            {
                Title = post.Title.Trim(),
                Content = post.Content, // keep formatting; trim if desired
                PublishedDate = now,
                LastUpdatedDate = now,
                Comments = new List<Comment>()
            };

            return await _blogPostRepository.CreateAsync(newPost, ct).ConfigureAwait(false);
        }

        public Task<BlogPost?> GetPostByIdAsync(int postId, CancellationToken ct = default)
        {
            if (postId <= 0) throw new ArgumentOutOfRangeException(nameof(postId));
            return _blogPostRepository.FindByIdAsync(postId, ct);
        }

        public Task DeletePostAsync(int id, CancellationToken ct = default)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));
            return _blogPostRepository.DeletePostAsync(id, ct);
        }

        public Task<IEnumerable<BlogPost>> GetAllBlogsAsync(
            Expression<Func<BlogPost, bool>>? filter = null,
            string? includeProperties = null,
            CancellationToken ct = default)
        {
            // Fall back to including Comments unless the caller specifies otherwise.
            var includes = string.IsNullOrWhiteSpace(includeProperties) ? DefaultIncludes : includeProperties;
            return _blogPostRepository.GetAllAsync(filter, includes, ct);
        }
    }
}
