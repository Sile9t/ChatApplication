using ChatCommon;
using System.Net;

namespace ChatApp
{
    public class Client
    {
        private readonly string _name;
        IClientMessageSource _messageSource;
        IPEndPoint _endPoint;
        bool _work = true;
        public Client(string name, string address, int port)
        {
            _name = name;
            _messageSource = new UdpClientMessageSource();
            _endPoint = new IPEndPoint(IPAddress.Parse(address), port);
        }
        public Client(string name, IClientMessageSource source, string address, int port)
        {
            _name = name;
            _messageSource = source;
            _endPoint = new IPEndPoint(IPAddress.Parse(address), port);
        }
        public async Task Start()
        {
            await Listen();
        }
        public void Stop() => _work = false;
        private async Task Listen()
        {
            Console.WriteLine("Client is waiting for message");
            while (_work)
            {
                try
                {
                    var messageReceived = _messageSource.Receive(ref _endPoint);
                    //Console.WriteLine(messageReceived);
                    Console.WriteLine($"Received message From '{messageReceived.From}':");
                    Console.WriteLine(messageReceived.Text);
                    await Confirm(messageReceived, _endPoint);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
        async Task Confirm(NetMessage msg, IPEndPoint endPoint)
        {
            msg.Command = Command.Confirmation;
            await _messageSource.SendAsync(msg, endPoint);
        }
        async Task Register(IPEndPoint endPoint)
        {
            var msg = new NetMessage("", Command.Register, _name, "");
            await _messageSource.SendAsync(msg, _endPoint);
        }
        
    }
}
