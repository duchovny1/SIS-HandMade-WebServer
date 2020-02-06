namespace SIS2
{
    using SIS.HTTP;

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    class Program
    {
        static async Task Main(string[] args)
        {

            var routeTable = new List<Route>();

            routeTable.Add(new Route(HttpMethodType.Get, "/", Index));
            routeTable.Add(new Route(HttpMethodType.Get, "/users/login", Login));
            routeTable.Add(new Route(HttpMethodType.Post, "/users/login", DoLogin));
            routeTable.Add(new Route(HttpMethodType.Get, "/contact", Contact));
            routeTable.Add(new Route(HttpMethodType.Get, "/favicon.ico", FavIcon));
            routeTable.Add(new Route(HttpMethodType.Get, "/headers", Headers));

            var httpServer = new HttpServer(1234, routeTable);
             await httpServer.StartAsync();
        }

        private static HttpResponse FavIcon(HttpRequest request)
        {


            var byteContent = File.ReadAllBytes("wwwroot/favicon.ico");

            return new FileResponse(byteContent, "image/x-icon");
        }

        public static HttpResponse Index(HttpRequest request)
        {
            var username = request.SessionData.ContainsKey("Username")
                                  ? request.SessionData["Username"] : "Anonymous";

            string content = $"<h1>index page,Hello {username}</h1>";
            return new HtmlResponse(content);
        }

        public static HttpResponse Login(HttpRequest request)
        {
            request.SessionData["Username"] = "Pesho";

            string content = "<h1>login page</h1>";
            return new HtmlResponse(content);
        }

        public static HttpResponse DoLogin(HttpRequest request)
        {
            string content = "<h1>do login page</h1>";
            return new HtmlResponse(content);
        }

        public static HttpResponse Contact(HttpRequest request)
        {
            string content = "<h1>contact page</h1>";
            return new HtmlResponse(content);
        }

        public static HttpResponse Headers(HttpRequest request)
        {

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<table border-collapse: collapse border: 1px solid black>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<th border: 1px solid black>Key</th>");
            sb.AppendLine("<th border: 1px solid black>Values</th>");
            sb.AppendLine("</tr>");

            foreach (var header in request.Headers)

            {
                sb.AppendLine("<tr>");

                sb.AppendLine($"<td border: 1px solid black>{header.Name}</td>");
                sb.AppendLine($"<td border: 1px solid black>{header.Value}</td>");
                sb.AppendLine("</tr>");
            }


            sb.AppendLine("</table>");


            return new HtmlResponse(sb.ToString());
        }
    }
}
