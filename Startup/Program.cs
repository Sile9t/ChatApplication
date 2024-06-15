using ChatApp;
using System.Net;

namespace ChatApplication
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                var server = new Server<IPEndPoint>(new UdpServerMessageSource());
                await server.Start();
            }
            else if (args.Length == 1)
            {
                var client = new Client<IPEndPoint>(args[0], new UdpClientMessageSource());
                await client.Start();
            }
            else
            {
                Console.WriteLine("To start server launch app with no aguments");
                Console.WriteLine("To start client launch app with client name");
            }
        }
    }
}
