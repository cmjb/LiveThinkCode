using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LiveThinkCode.Models
{
    public class Article
    {

        public Article()
        {
            Tags = new List<Tag>();
            Categories = new List<Category>();
        }

        [Key]
        public string ArticleId { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public ApplicationUser Author { get; set; }
        public string Slug { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public bool Active { get; set; }

        public ICollection<Tag> Tags { get; set; }
        public ICollection<Category> Categories { get; set; }
    }
}
