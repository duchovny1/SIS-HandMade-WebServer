using SIS.HTTP;
using SIS2;
using SulsApp.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SulsApp
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var db = new ApplicationDbContext();
            db.Database.EnsureCreated();

            var routeTable = new List<Route>();

            routeTable.Add(new Route(HttpMethodType.Get, "/", new HomeController().Index));
            

            var httpServer = new HttpServer(80, routeTable);
            await httpServer.StartAsync();
        }
    }
}
