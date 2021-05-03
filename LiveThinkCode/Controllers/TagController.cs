using LiveThinkCode.Data;
using LiveThinkCode.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveThinkCode.Controllers
{
    public class TagController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TagController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult Search()
        {
            var terms = Request.Query["term"];
            IQueryable<Tag> tags = _db.Tags.Where(x => x.Name.Contains(terms));
            IList<Tag> tagList = tags.ToList();
            List<string> tagNames = new();
            foreach(Tag tag in tagList)
            {
                tagNames.Add(tag.Name);
            }
            return Json(tagNames);
        }

        [HttpGet]
        public JsonResult GetTagsForArticle()
        {
            var term = Request.Query["term"];
            Article article = _db.Articles.First(x => x.Title.Contains(term));
            IQueryable<Tag> tags = _db.Tags.Where(x => x.Articles.Contains(article));
            return Json(tags);
        }

        [HttpGet]
        public JsonResult GetRandomTags()
        {
            IQueryable<Tag> tags = _db.Tags.OrderBy(r => Guid.NewGuid()).Take(5);
            return Json(tags.ToList());
        }
    }
}
