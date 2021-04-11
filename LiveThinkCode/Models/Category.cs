using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LiveThinkCode.Models
{
    public class Category
    {
        [Key]
        public string CategoryId { get; set; }
        public string Title { get; set; }
    }
}
