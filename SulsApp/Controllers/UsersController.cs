using SIS.HTTP;
using SIS.MvcFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SulsApp.Controllers
{
    public class UsersController : Controller
    {
        [HttpGet]
        public HttpResponse Login(HttpRequest request)
        {
            return this.View();
        }

        [HttpPost("/Users/Login")]
        public HttpResponse DoLogin(HttpRequest request)
        {
            
            return this.View();
        }

        [HttpGet]
        public HttpResponse Register(HttpRequest request)
        {

            return this.View();
        }

        [HttpPost("/Users/Register")]
        public HttpResponse DoRegister(HttpRequest request)
        {

            return this.View();
        }
    }
}
