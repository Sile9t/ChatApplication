using ChatApp.ClientMessageSource;
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
            _endPoint = _messageSource.GetServer();
        }

        public async Task Start()
        {
            new Thread( async _ => await SendAsync() ).Start();
            new Thread( async _ => Listen() ).Start();
        }

        public void Stop() => _work = false;

        private async void Listen()
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

        private async Task SendAsync()
        {
            Register(_endPoint);
            while (_work)
            {
                try
                {
                    Console.WriteLine("Enter recipient name:");
                    var recName = Console.ReadLine();

                    Console.WriteLine("Enter message text:");
                    var mesText = Console.ReadLine();

                    var message = new NetMessage(mesText, Command.Message, _name, recName);

                    await _messageSource.SendAsync(message, _endPoint);

                    Console.WriteLine("Messsage sent");
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

        async Task Register(T endPoint)
        {
            var msg = new NetMessage("", Command.Register, _name, "");

            await _messageSource.SendAsync(msg, endPoint);
        }
    }
}
