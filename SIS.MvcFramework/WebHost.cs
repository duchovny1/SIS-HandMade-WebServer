using SIS.HTTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SIS.MvcFramework
{
    public static class WebHost
    {
        public static async Task StartASync(IMvcApplication application)
        {
            var routeTable = new List<Route>();

            application.ConfigureServices();
            application.Configure(routeTable);
            AutoRegisterStaticFilesRoutes(routeTable);
            AutoRegisterActionRoutes(routeTable, application);

            var httpServer = new HttpServer(80, routeTable);

            await httpServer.StartAsync();
        }

        private static void AutoRegisterActionRoutes(List<Route> routeTable, IMvcApplication application)
        {
            var types = application.GetType().Assembly.GetTypes()
                .Where(type => type.IsSubclassOf(typeof(Controller)));

            foreach (var type in types)
            {
                var methods = type.GetMethods().Where(x => !x.IsSpecialName && !x.IsConstructor
                && x.DeclaringType == type);
                foreach (var method in methods)
                {
                    string url = type.Name.Replace("Controller", string.Empty) + "/" + method.Name;
                    var attribute = method.GetCustomAttributes().FirstOrDefault(x =>
                        x.GetType().IsSubclassOf(typeof(HttpMethodAttribute)))
                        as HttpMethodAttribute;

                    var httpActionType = HttpMethodType.Get;
                    if(attribute != null)
                    {
                        httpActionType = attribute.Type;
                        if(attribute.Url != null)
                        {
                            url = attribute.Url;
                        }
                    }

                    routeTable.Add(new Route(HttpMethodType.Get, url, (request) =>
                    {
                        var controller = Activator.CreateInstance(type) as Controller;
                        controller.Request = request;
                        var response = method.Invoke(controller, new object[] { }) as HttpResponse;
                        return response;
                    }));
                }
            }
        }

        private static void AutoRegisterStaticFilesRoutes(List<Route> routeTable)
        {
            var staticFiles = Directory.GetFiles("wwwroot", "*", SearchOption.AllDirectories);

            foreach (var file in staticFiles)
            {
                var path = file.Replace(@"wwwroot", string.Empty).Replace("\\", "/") ;
                routeTable.Add(new Route(HttpMethodType.Get, path, (request) =>
                {
                    var fileInfo = new FileInfo(file);
                    var contentType = fileInfo.Extension switch
                    {
                        ".css" => "",
                        ".html" => "text/html",
                        ".js" => "text/javascript",
                        ".ico" => "image/x-icon",
                        ".jpg" => "image/jpeg",
                        ".jpeg" => "image/jpeg",
                        ".png" => "image/png",
                        ".gif" => "image/gif",
                        _ => "text/plain",
                    };
                    return new FileResponse(File.ReadAllBytes(file), contentType);

                }));
            }
        }
    }
}
