using BlogService.Core.Services.Interfaces;
using BlogService.DataAccess.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PostsController : ControllerBase
    {
        private readonly IBlogPostService _blogPostService;

        public PostsController(IBlogPostService blogPostService)
        {
            _blogPostService = blogPostService;
        }

       // Create a new blog post
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
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // Get a blog post by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPostById(int id)
    {
        try
        {
            var post = await _blogPostService.GetPostByIdAsync(id);
            if (post == null)
                return NotFound($"No Post Found with Id: {id}");

            return Ok(post);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // Get all blog posts
    [HttpGet]
    public async Task<IActionResult> GetAllPosts()
    {
        try
        {
            var result = await _blogPostService.GetAllBlogsAsync();
            if (result == null || !result.Any())
                return NotFound("No Blogs Found");

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // Update an existing blog post
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePost(int id, [FromBody] BlogPostDTO postDTO)
    {
        try
        {
            var updatedPost = await _blogPostService.UpdatePostAsync(id, postDTO);
            if (updatedPost == null)
                return NotFound($"No Post Found with Id: {id}");

            return Ok(updatedPost);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // Delete a blog post
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        try
        {
            var isDeleted = await _blogPostService.DeletePostAsync(id);
            if (!isDeleted)
                return NotFound($"No Post Found with Id: {id}");

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // Search for blog posts by keyword
    [HttpGet("search")]
    public async Task<IActionResult> SearchPosts([FromQuery] string keyword)
    {
        try
        {
            var results = await _blogPostService.SearchPostsAsync(keyword);
            if (results == null || !results.Any())
                return NotFound($"No posts found containing '{keyword}'");

            return Ok(results);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // Get posts filtered by category
    [HttpGet("category/{categoryName}")]
    public async Task<IActionResult> GetPostsByCategory(string categoryName)
    {
        try
        {
            var results = await _blogPostService.GetPostsByCategoryAsync(categoryName);
            if (results == null || !results.Any())
                return NotFound($"No posts found in category '{categoryName}'");

            return Ok(results);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}

    }
}
