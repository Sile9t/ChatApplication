using ChatCommon;
using System.Net;

namespace ChatApp
{
    public class Client<T>
    {
        private readonly string _name;
        IClientMessageSource<T> _messageSource;
        T _endPoint;
        bool _work = true;

        public Client(string name, IClientMessageSource<T> source)
        {
            _name = name;
            _messageSource = source;
            _endPoint = _messageSource.CreateEndPoint();
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

        async Task Confirm(NetMessage msg, T endPoint)
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
