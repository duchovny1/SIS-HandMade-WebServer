namespace SIS.HTTP
{

    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    public class HttpServer : IHttpServer
    {
        private readonly TcpListener tcpListener;
        public HttpServer(int port)
        {
            tcpListener = new TcpListener(IPAddress.Loopback, port);
            //StartAsync().GetAwaiter().GetResult();
           
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

                Task.Run(() => ProcessClientAsync(tcpClient));


            }
        }

        public void Stop()
        {
            this.tcpListener.Stop();
        }

        private async Task ProcessClientAsync(TcpClient tcp)
        {
            
            //put it in using
            using NetworkStream networkStream = tcp.GetStream();

            byte[] requestBytes = new byte[100000];

            int bytesRead = await networkStream.ReadAsync(requestBytes, 0, requestBytes.Length);

            string requestAsString = Encoding.UTF8.GetString(requestBytes, 0, requestBytes.Length);
            string content = "<h1>random page</h1>";

            var request = new HttpRequest(requestAsString);

            if (request.Path == "/")
            {
                content = "<h1>home page</h1>";
            }
            else if(request.Path == "/users/login")
            {
                content = "<h1>login page </h1>";
            }


           
            byte[] fileContext = Encoding.UTF8.GetBytes(content);
            var response = new HttpResponse(StatusCode.Ok, fileContext);
            response.Headers.Add(new Header("Server", "SoftUni/1.1"));
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
    }
}
