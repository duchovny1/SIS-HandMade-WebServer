namespace SIS2
{
    using SIS.HTTP;

    using System;

    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class StartUp
    {
        static async Task Main(string[] args)
        {

            var db = new ApplicationDbContext();
            db.Database.EnsureCreated();

            var routeTable = new List<Route>();

            routeTable.Add(new Route(HttpMethodType.Get, "/", Index));
            routeTable.Add(new Route(HttpMethodType.Post, "Tweets/Create", CreatTweet));
            routeTable.Add(new Route(HttpMethodType.Get, "/favicon.ico", FavIcon));

            var httpServer = new HttpServer(1234, routeTable);
            await httpServer.StartAsync();
        }

        private static HttpResponse CreatTweet(HttpRequest request)
        {
            var db = new ApplicationDbContext();
            db.Tweets.Add(new Tweet
            {
                CreatedOn = DateTime.UtcNow,
                Creator = request.FormData["creator"],
                Content = request.FormData["tweetName"]
            });

            db.SaveChanges();
            return new RedirectResponse("/");
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

            var db = new ApplicationDbContext();

            var tweets = db.Tweets.Select(x => new
            {
                x.CreatedOn,
                x.Creator,
                x.Content
            }).ToList();
            StringBuilder html = new StringBuilder();

            html.Append("<table><tr><th>Date</th><th>Creator</th><th>Content</th></tr>");

            foreach (var tweet in tweets)
            {
                html.Append($"<tr><td>{tweet.CreatedOn}</td><td{tweet.Creator}></td><td>{tweet.Content}</td></tr>");
            }

            html.Append("</table>");
            html.Append($"<form action= '/Tweets/Create' method='post'>" +
                $"<input name='creator' /> + < /br>" +
                $"<textarea name='tweetName'></textarea> < /br>" +
                $"<input type='submit' /></form>");

            return new HtmlResponse(html.ToString());
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
