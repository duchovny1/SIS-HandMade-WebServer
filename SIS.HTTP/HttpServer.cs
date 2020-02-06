namespace SIS.HTTP
{

    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Net.Sockets;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Linq;

    public class HttpServer : IHttpServer
    {
        private readonly TcpListener tcpListener;
        private readonly IList<Route> routeTable;
        private IDictionary<string, Dictionary<string, string>> sessions;
        public HttpServer(int port, IList<Route> routeTable)
        {
            tcpListener = new TcpListener(IPAddress.Loopback, port);
            this.routeTable = routeTable;
            this.sessions = new Dictionary<string, Dictionary<string, string>>();
            StartAsync().GetAwaiter().GetResult();
        }

        public async Task ResetAsync()
        {
            this.Stop();
            await this.StartAsync();
        }

        public async Task StartAsync()
        {
            tcpListener.Start();

            while (true)
            {
                TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();

                _ = Task.Run(() => ProcessClientAsync(tcpClient));


            }
        }

        public void Stop()
        {
            this.tcpListener.Stop();
        }

        private async Task ProcessClientAsync(TcpClient tcp)
        {

            using NetworkStream networkStream = tcp.GetStream();
            //put it in using
            try
            {

                byte[] requestBytes = new byte[100000];

                int bytesRead = await networkStream.ReadAsync(requestBytes, 0, requestBytes.Length);

                string requestAsString = Encoding.UTF8.GetString(requestBytes, 0, requestBytes.Length);

                var request = new HttpRequest(requestAsString);
                string newSessionId = null;
                var sessionCookie = request.Cookies
                   .FirstOrDefault(x => x.Name == HttpConstants.SessionIdCookieName);
                if (sessionCookie != null && this.sessions.ContainsKey(sessionCookie.Value))
                {
                    request.SessionData = this.sessions[sessionCookie.Value];
                }
                else
                {
                    newSessionId = Guid.NewGuid().ToString();
                    var dictionary = new Dictionary<string, string>();
                    this.sessions.Add(newSessionId, dictionary);
                    request.SessionData = dictionary;
                }

                Console.WriteLine($"{request.Method} {request.Path}");

                var route = this.routeTable.FirstOrDefault(x => x.HttpMethod == request.Method
                && x.Path == request.Path);

                HttpResponse response;
                

                if (route == null)
                {
                    response = new HttpResponse(StatusCode.NotFound, new byte[0]);
                }
                else
                {
                    response = route.Action(request);
                }

                response.Headers.Add(new Header("Server", "SoftUni/1.1"));



                if (newSessionId != null)
                {

                    response.Cookies.Add(new ResponseCookie(HttpConstants.SessionIdCookieName,
                        newSessionId)
                    { HttpOnly = true, MaxAge = 30 * 3600 });
                }
                response.Headers.Add(new Header("Content-type", "text/html"));



                //string responeString = "HTTP/1.1 200 OK" + HttpConstants.NewLine
                //    + "Server: SoftUni1.1" + HttpConstants.NewLine
                //    + "Content-type: text/html" + HttpConstants.NewLine
                //    + "Content-Length: " + fileContext.Length + HttpConstants.NewLine
                //    + HttpConstants.NewLine;

                byte[] headersBytes = Encoding.UTF8.GetBytes(response.ToString());

                await networkStream.WriteAsync(headersBytes, 0, headersBytes.Length);
                await networkStream.WriteAsync(response.Content, 0, response.Content.Length);

                Console.WriteLine(request);

            }
            catch (Exception ex)
            {
                var errorResponse = new HttpResponse(StatusCode.InternalServerError,
                    Encoding.UTF8.GetBytes(ex.Message));

                errorResponse.Headers.Add(new Header("Content-type", "text/plain"));
                byte[] responseBytes = Encoding.UTF8.GetBytes(errorResponse.ToString());

                await networkStream.WriteAsync(responseBytes, 0, responseBytes.Length);
                await networkStream.WriteAsync(errorResponse.Content, 0, errorResponse.Content.Length);

            }


        }
    }
}
