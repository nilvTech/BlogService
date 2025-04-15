using BlogService.Core.Services.Interfaces;
using BlogService.DataAccess.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BlogService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PostsController : ControllerBase
    {
        private readonly IBlogPostService _blogPostService;
        private readonly ILogger<PostsController> _logger;

        public PostsController(IBlogPostService blogPostService, ILogger<PostsController> logger)
        {
            _blogPostService = blogPostService;
            _logger = logger;
        }

        /// <summary>
        /// Create a new blog post.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] BlogPostDTO postDTO)
        {
            try
            {
                var createdPost = await _blogPostService.CreatePostAsync(postDTO);
                return CreatedAtAction(nameof(GetPostById), new { id = createdPost.Id }, createdPost);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a post.");
                return StatusCode(500, "An error occurred while creating the blog post.");
            }
        }

        /// <summary>
        /// Get a blog post by its ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            try
            {
                var post = await _blogPostService.GetPostByIdAsync(id);
                return post != null ? Ok(post) : NotFound($"No post found with ID {id}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving post with ID {id}.");
                return StatusCode(500, "An error occurred while retrieving the post.");
            }
        }

        /// <summary>
        /// Get all blog posts.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            try
            {
                var posts = await _blogPostService.GetAllBlogsAsync();
                return posts != null && posts.Any() ? Ok(posts) : NotFound("No blog posts found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all posts.");
                return StatusCode(500, "An error occurred while retrieving blog posts.");
            }
        }

        /// <summary>
        /// Update a blog post.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(int id, [FromBody] BlogPostDTO postDTO)
        {
            try
            {
                var updatedPost = await _blogPostService.UpdatePostAsync(id, postDTO);
                return updatedPost != null ? Ok(updatedPost) : NotFound($"No post found with ID {id}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating post with ID {id}.");
                return StatusCode(500, "An error occurred while updating the post.");
            }
        }

        /// <summary>
        /// Delete a blog post by ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            try
            {
                var deleted = await _blogPostService.DeletePostAsync(id);
                return deleted ? NoContent() : NotFound($"No post found with ID {id}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting post with ID {id}.");
                return StatusCode(500, "An error occurred while deleting the post.");
            }
        }

        /// <summary>
        /// Search blog posts by keyword.
        /// </summary>
        [HttpGet("search")]
        public async Task<IActionResult> SearchPosts([FromQuery] string keyword)
        {
            try
            {
                var results = await _blogPostService.SearchPostsAsync(keyword);
                return results != null && results.Any() ? Ok(results) : NotFound($"No posts found containing '{keyword}'.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error searching posts with keyword '{keyword}'.");
                return StatusCode(500, "An error occurred while searching blog posts.");
            }
        }

        /// <summary>
        /// Get posts by category name.
        /// </summary>
        [HttpGet("category/{categoryName}")]
        public async Task<IActionResult> GetPostsByCategory(string categoryName)
        {
            try
            {
                var results = await _blogPostService.GetPostsByCategoryAsync(categoryName);
                return results != null && results.Any() ? Ok(results) : NotFound($"No posts found in category '{categoryName}'.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving posts for category '{categoryName}'.");
                return StatusCode(500, "An error occurred while retrieving posts by category.");
            }
        }
    }
}
