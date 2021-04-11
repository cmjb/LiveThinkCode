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
        private IWebHostEnvironment _environment;
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
        public JsonResult Upload(IFormFile file)
        {
            var uploads = Path.Combine(_environment.WebRootPath, "assets");



            /*foreach (var file in files)
            {
                if (file.Length > 0)
                {

                }
            }*/
            /*Dictionary<string, object> result = null;
            using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
            {
                var task = file.CopyToAsync(fileStream);
                task.Wait();
                result = new Dictionary<string, object>
                        {
                            { "resourceType", "Files" },
                            { "currentFolder", new Dictionary<string, object>
                                {
                                    { "path", "/"},
                                    { "url", "/assets/"},
                                    { "acl", 255}
                                }
                            },
                            { "fileName", file.FileName },
                            { "uploaded", 1 }
                        };
            }*/

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
