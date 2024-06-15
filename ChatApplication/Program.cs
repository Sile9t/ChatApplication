using System.Net;

namespace ChatApplication
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                var server = new Server<IPEndPoint>(new UdpServerMessageSource());
                server.Start();
            }
        }
    }
}
