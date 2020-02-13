using SIS.HTTP;
using SIS.MvcFramework;
using SulsApp.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SulsApp.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("/")]

        public HttpResponse Index(HttpRequest request)
        {
            var viewModel = new IndexViewModel
            {
                Message = "Welcome to SULS Platform!",
                Year = DateTime.UtcNow.Year,
            };

            
            return this.View(viewModel);
        }
    }
}
