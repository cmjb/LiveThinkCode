using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LiveThinkCode.Controllers
{
    public class FileController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        public FileController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResult Upload()
        {
            var uploads = Path.Combine(_environment.WebRootPath, "assets");

            try
            {


                var filePath = Path.Combine(uploads, "assets");
                var urls = new List<string>();
                if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);

                foreach (var contentFile in Request.Form.Files)
                {
                    if (contentFile != null && contentFile.Length > 0)
                    {
                        contentFile.CopyTo(new FileStream($"{filePath}\\{contentFile.FileName}", FileMode.Create));
                        urls.Add($"{HttpContext.Request.Host}/assets/{contentFile.FileName}");
                    }
                }

                return Json(urls);
            }
            catch (Exception e)
            {
                return Json(new { error = new { message = e.Message } });
            }
        }
    }
}
