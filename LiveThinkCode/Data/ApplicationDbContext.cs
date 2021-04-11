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
        }

        public DbSet<Article> Articles { get; set; }
    }
}
