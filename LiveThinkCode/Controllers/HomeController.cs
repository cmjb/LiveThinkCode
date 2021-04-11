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

namespace LiveThinkCode.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
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
    }
}
