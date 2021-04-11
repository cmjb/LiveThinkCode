using LiveThinkCode.Data;
using LiveThinkCode.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace LiveThinkCode.Controllers
{
    public class ArticleController : Controller
    {

        private ApplicationDbContext _db;

        public ArticleController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: ArticleController
        public ActionResult Index()
        {
            var articles = _db.Articles.ToList();
            return View(articles);
        }

        // GET: ArticleController/Details/test
        public ActionResult Details(string id)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            Article article = _db.Articles.First(x => x.Title == id);


            return View(article);
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
        public ActionResult Create([FromForm] Article article)
        {
            //var model = new Article();
            //var task = TryUpdateModelAsync<Article>(model);

            //task.Wait();
            //var result = task.Result;
            article.CreationDate = DateTime.Now;
            article.ModificationDate = DateTime.Now;
            if(article != null)
            {
                _db.Articles.Add(article);
                _db.SaveChanges();
            }

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
    }
}
