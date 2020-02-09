using SIS.HTTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SulsApp.Controllers
{
    public class HomeController
    {
        public HttpResponse Index(HttpRequest request)
        {
            var html = File.ReadAllText("Views/Home/Index.html");
            return new HtmlResponse(html);
        }
    }
}
