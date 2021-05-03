using LiveThinkCode.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiveThinkCode.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Article>()
            .Property(f => f.ArticleId)
            .ValueGeneratedOnAdd();
            modelBuilder.Entity<Tag>()
            .Property(f => f.TagId)
            .ValueGeneratedOnAdd();
            modelBuilder.Entity<Category>()
            .Property(f => f.CategoryId)
            .ValueGeneratedOnAdd();

            modelBuilder.Entity<Article>().HasMany(a => a.Tags).WithMany(t => t.Articles);
            modelBuilder.Entity<Article>().HasMany(a => a.Categories).WithMany(c => c.Articles);
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Category> Categories { get; set; }

    }
}
