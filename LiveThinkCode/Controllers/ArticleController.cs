using LiveThinkCode.Data;
using LiveThinkCode.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace LiveThinkCode.Controllers
{
    public class ArticleController : Controller
    {

        private readonly ApplicationDbContext _db;

        private readonly ILogger<ArticleController> _logger;

        public ArticleController(ApplicationDbContext db, ILogger<ArticleController> logger)
        {
            _db = db;
            _logger = logger;
        }

        // GET: ArticleController
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var articles = _db.Articles.ToList();
            foreach(var article in articles)
            {
                IQueryable<Tag> tags = _db.Tags.Where(x => x.Articles.Contains(article));
                IQueryable<Category> categories = _db.Categories.Where(x => x.Articles.Contains(article));
                foreach (var tag in tags)
                {
                    _logger.LogInformation("Tag name {tag}", tag.Name);
                }

                article.Tags = tags.ToList();
                foreach (var tag in article.Tags)
                {
                    _logger.LogInformation("Tag name {tag}", tag.Name);
                }

                article.Categories = categories.ToList();
            }
            return View(articles);
        }

        // GET: ArticleController/Show/test
        public ActionResult Show(string id)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            Article article = _db.Articles.Where(x => x.Active).First(x => x.Title == id);

            if(article.Active == false)
            {
                return Forbid();
            }
            IQueryable<Tag> tags = _db.Tags.Where(x => x.Articles.Contains(article));
            IQueryable<Category> categories = _db.Categories.Where(x => x.Articles.Contains(article));
            foreach (var tag in tags)
            {
                _logger.LogInformation("Tag name {tag}", tag.Name);
            }

            article.Tags = tags.ToList();
            article.Categories = categories.ToList();
            return View(article);
        }

        public ActionResult ArticlesByTag(string id)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var articlesLoad = _db.Articles.Where(x => x.Active).ToList();
            foreach (var article in articlesLoad)
            {
                IQueryable<Tag> tags = _db.Tags.Where(x => x.Articles.Contains(article) );
                IQueryable<Category> categories = _db.Categories.Where(x => x.Articles.Contains(article));
                foreach (var tag in tags)
                {
                    _logger.LogInformation("Tag name {tag}", tag.Name);
                }

                article.Tags = tags.ToList();
                foreach (var tag in article.Tags)
                {
                    _logger.LogInformation("Tag name {tag}", tag.Name);
                }

                article.Categories = categories.ToList();
            }

            var queryableArticles = articlesLoad.AsQueryable();

            Tag requestedTag = _db.Tags.First(x => x.Name == id);
            var articles = queryableArticles.Where(x => x.Tags.Contains(requestedTag));

            return View(articles);
        }

        // GET: ArticleController/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: ArticleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(IFormCollection formCollection)
        {
            var model = new Article
            {
                Title = formCollection["Title"],
                Summary = formCollection["Summary"],
                Content = formCollection["Content"],
                Slug = formCollection["Slug"]
            };
            string tags = formCollection["Tags"];
            string category = formCollection["Categories"];
            _logger.LogInformation("Tickbox result {result}", formCollection["Active"]);
            _logger.LogInformation("Tickbox result {result}", formCollection["Active"].ToString());
            if (formCollection["Active"].ToString() == "false")
                model.Active = false;
            else
                model.Active = true;
            
            String[] strlist = tags.Split(",", 10,
               StringSplitOptions.RemoveEmptyEntries);

            foreach(string str in strlist)
            {

                Tag tag;

                try
                {
                     tag = _db.Tags.First(x => x.Name == str);
                }
                catch(InvalidOperationException ex)
                {
                    _logger.LogInformation("Can't find tag, create one. Exception: ", ex.Message);
                    tag = new Tag { Name = str };
                }
                model.Tags.Add(tag);
                //model.Tags.Attach(tag);
            }

            String[] categoryLists = category.Split(",", 10,
                StringSplitOptions.RemoveEmptyEntries);

            foreach (string str in categoryLists)
            {

                Category categoryEntity;

                try
                {
                    categoryEntity = _db.Categories.First(x => x.Title == str);
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogInformation("Can't find tag, create one. Exception: ", ex.Message);
                    categoryEntity = new Category { Title = str };
                }
                model.Categories.Add(categoryEntity);
            }

            model.CreationDate = DateTime.Now;
            model.ModificationDate = DateTime.Now;
            foreach (var tag in model.Tags)
            {
                _logger.LogInformation("Tag name {tag}", tag.Name);
            }
            _db.Articles.Add(model);
            _db.SaveChanges();
            

            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ArticleController/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            Article article = _db.Articles.First(x => x.Title == id);
            return View(article);
        }

        // POST: ArticleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id, IFormCollection collection)
        {
            Article article = _db.Articles.First(x => x.Title == id);
            
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ArticleController/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            Article article = _db.Articles.First(x => x.Title == id);
            
            return View(article);
        }

        // POST: ArticleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id, IFormCollection collection)
        {
            Article article = _db.Articles.First(x => x.Title == id);
            _db.Articles.Remove(article);
            _db.SaveChanges();
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public JsonResult GetRandomArticles()
        {
            IQueryable<Article> articles = _db.Articles.Where(x => x.Active).OrderBy(r => Guid.NewGuid()).Take(5);
            return Json(articles.ToList());
        }
    }
}
