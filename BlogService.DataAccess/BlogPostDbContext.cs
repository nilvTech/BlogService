﻿using BlogService.DataAccess.Models;
using BlogService.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogService.DataAccess
{
    public class BlogPostDbContext : DbContext
    {
        public BlogPostDbContext(DbContextOptions<BlogPostDbContext> options) : base(options) 
        {
            
        }

        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Properties> Properties { get; set; }

    }
}
