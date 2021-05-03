using LiveThinkCode.Data;
using LiveThinkCode.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LiveThinkCode.Controllers;

namespace LiveThinkCode.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db, ILogger<HomeController> logger, RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _logger = logger;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index(int? pageNumber)
        {
            _logger.LogInformation("page number {nom}", pageNumber);
            int pageSize = 3;
            int page = (pageNumber ?? 1);
            var articles = _db.Articles.Where(x => x.Active == true).OrderByDescending(s => s.CreationDate);
            /*
            foreach (var article in articles)
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
            }*/

            return View(PaginatedList<Article>.CreateAsync(articles, page, pageSize).Result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [Authorize]
        public IActionResult AddAdmin()
        {
            var userPrincipal = this.User;
            var task = Task.Run(async () =>
            {
               var user = await _userManager.GetUserAsync(userPrincipal);
               bool userRoleExists = await _roleManager.RoleExistsAsync("Admin");

               if (!userRoleExists)
               {
                   var roleResult = await _roleManager.CreateAsync(new ApplicationRole { Name = "Admin" });
               }

               var roleresult = await _userManager.AddToRoleAsync(user, "Admin");
               _logger.LogInformation("User added to role 'Admins'.");
            });

            task.Wait();
           
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public List<Article> GetRandomArticles()
        {
            IQueryable<Article> articles = _db.Articles.OrderBy(r => Guid.NewGuid()).Take(5);
            return articles.ToList();
        }
    }
}
