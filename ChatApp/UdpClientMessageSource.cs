using ChatCommon;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace ChatApp
{
    public class UdpClientMessageSource : IClientMessageSource<IPEndPoint>
    {
        private readonly UdpClient _udpClient;
        private readonly IPEndPoint _udpEndPoint;
        public UdpClientMessageSource(string ip = "127.0.0.1", int port = 0)
        {
            _udpClient = new UdpClient(12345);
            _udpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        }

        public IPEndPoint CreateEndPoint()
        {
            return new IPEndPoint(IPAddress.Any, 0);
        }

        public IPEndPoint GetServer()
        {
            return _udpEndPoint;
        }

        public NetMessage Receive(ref IPEndPoint endPoint)
        {
            var buffer = _udpClient.Receive(ref endPoint);
            var text = Encoding.UTF8.GetString(buffer);

            return NetMessage.DeserializeFromJson(text) ?? new NetMessage();
        }

        public async Task SendAsync(NetMessage msg, IPEndPoint endPoint)
        {
            var text = msg.SerializeToJson();
            var buffer = Encoding.UTF8.GetBytes(text);

            await _udpClient.SendAsync(buffer);
        }
    }
}
