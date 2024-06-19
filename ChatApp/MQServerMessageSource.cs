using ChatCommon;
using NetMQ;
using NetMQ.Sockets;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatApp
{
    public class MQServerMessageSource : IServerMessageSource<IPEndPoint>
    {
        private readonly UdpClient _udpClient;
        public MQServerMessageSource()
        {
            _udpClient = new UdpClient(12346);
        }
        public IPEndPoint CopyEndPoint(IPEndPoint endPoint)
        {
            return new IPEndPoint(endPoint.Address,  endPoint.Port);
        }

        public IPEndPoint CreateEndPoint()
        {
            return new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
        }

        public NetMessage Receive(ref IPEndPoint endPoint)
        {
            using (var response = new ResponseSocket(GetAddressString(endPoint)))
            {
                var buffer = response.ReceiveFrameBytes();
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
