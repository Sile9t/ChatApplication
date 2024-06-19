using ChatCommon;
using NetMQ;
using NetMQ.Sockets;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatApp
{
    public class MQClientMessageSource : IClientMessageSource<IPEndPoint>
    {
        private readonly UdpClient _udpClient;
        private readonly IPEndPoint _udpEndPoint;
        public MQClientMessageSource(string address = "127.0.0.1", int port = 12346)
        {
            _udpClient = new UdpClient(12345);
            _udpEndPoint = new IPEndPoint(IPAddress.Parse(address), port);
        }
        public IPEndPoint CreateEndPoint()
        {
            return new IPEndPoint(_udpEndPoint.Address, _udpEndPoint.Port);
        }

        public IPEndPoint GetServer()
        {
            return _udpEndPoint;
        }

        public NetMessage Receive(ref IPEndPoint endPoint)
        {
            using (var responce = new ResponseSocket(GetAddressString(endPoint)))
            {
                var buffer = responce.ReceiveFrameBytes();
                var text = Encoding.UTF8.GetString(buffer);

                return NetMessage.DeserializeFromJson(text) ?? new NetMessage();
            }
        }

        public Task SendAsync(NetMessage msg, IPEndPoint endPoint)
        {
            using (var request = new RequestSocket(GetAddressString(endPoint)))
            {
                var text = msg.SerializeToJson();
                var buffer = Encoding.UTF8.GetBytes(text);

                return Task.Run(() => request.SendFrame(buffer));
            }
        }

        private string GetAddressString(IPEndPoint endPoint)
        {
            return $"@udp://{endPoint.Address}:{endPoint.Port}";
        }
    }
}
