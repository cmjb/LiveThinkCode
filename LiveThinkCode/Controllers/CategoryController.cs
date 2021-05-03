using LiveThinkCode.Data;
using LiveThinkCode.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveThinkCode.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View(_db.Categories.ToList());
        }

        [HttpGet]
        public JsonResult Search()
        {
            var terms = Request.Query["term"];
            IQueryable<Category> categories = _db.Categories.Where(x => x.Title.Contains(terms));
            IList<Category> categoriesList = categories.ToList();
            List<string> categoriesNames = new();
            foreach (Category category in categoriesList)
            {
                categoriesNames.Add(category.Title);
            }
            return Json(categoriesNames);
        }
    }
}
