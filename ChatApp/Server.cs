using ChatCommon;
using ChatDB;
using System.Net;
using System.Net.Sockets;

namespace ChatApp
{
    public class Server<T>
    {
        Dictionary<string, T> clients = new ();
        private readonly IServerMessageSource<T> _messageSource;
        private T _endPoint;
        public Server(IServerMessageSource<T> messageSource)
        {
            _messageSource = messageSource;
            _endPoint = _messageSource.CreateEndPoint();
        }
        async Task ProcessMessage(NetMessage message)
        {
            if (message == null) return;
            switch (message.Command)
            {
                case Command.Register:
                    await Register(message);
                    break;
                case Command.Message:
                    await Reply(message);
                    break;
                case Command.Confirmation:
                    await ConfirmMessageReceived(message.Id);
                    break;
            }
        }
        private async Task Register(NetMessage message)
        {
            Console.WriteLine($"Message register {message.From}");
            if (clients.TryAdd(message.From, _endPoint))
            {
                using (var context = new ChatContext())
                {
                    context.Users.Add(new User { FullName = message.From });
                    await context.SaveChangesAsync();
                }
            }
        }
        private async Task Reply(NetMessage message)
        {
            if (clients.TryGetValue(message.To, out T ep))
            {
                int id = 0;
                using (var context = new ChatContext())
                {
                    var fromUser = context.Users.First(x => x.FullName == message.From);
                    var toUser = context.Users.First(x => x.FullName == message.To);
                    var msg = new Message
                    {
                        From = fromUser,
                        To = toUser,
                        IsSent = false,
                        Text = message.Text
                    };
                    context.Messages.Add(msg);
                    context.SaveChanges();
                    id = msg.Id;
                }
                message.Id = id;
                await _messageSource.SendAsync(message, ep);
                Console.WriteLine($"Message replied From : {message.From} To : {message.To}");
            }
            else Console.WriteLine("User not found");
        }
        async Task ConfirmMessageReceived(int id)
        {
            Console.WriteLine($"Message comfirmation id : {id}");
            using (var context = new ChatContext())
            {
                var msg = context.Messages.FirstOrDefault(x => x.Id == id);
                if (msg != null)
                {
                    msg.IsSent = true;
                    await context.SaveChangesAsync();
                }
            }
        }
        public async Task Listen()
        {
            Console.WriteLine("Server is waiting for message");

            while (_work)
            {
                try
                {
                    var msg = _messageSource.Receive(ref _endPoint);
                    Console.WriteLine(msg);
                    await ProcessMessage(msg);
                }
                catch (Exception ex) 
                {
                    Console.WriteLine(ex);
                }
            }
        }
        bool _work = true;
        public void Stop()
        {
            _work = false;
        }
    }
}
