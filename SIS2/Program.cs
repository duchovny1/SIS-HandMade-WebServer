namespace SIS2
{
    using SIS.HTTP;
    using System.Threading.Tasks;

    class Program
    {
        static async Task Main(string[] args)
        {
            var httpServer = new HttpServer(1234);
            await httpServer.StartAsync();
        }
    }
}
