﻿using ChatCommon;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatApp
{
    public class UdpServerMessageSource : IServerMessageSource<IPEndPoint>
    {
        private readonly UdpClient _udpClient;
        public UdpServerMessageSource()
        {
            _udpClient = new UdpClient(12346);
        }

        public IPEndPoint CopyEndPoint(IPEndPoint endPoint)
        {
            return new IPEndPoint(endPoint.Address, endPoint.Port);
        }

        public IPEndPoint CreateEndPoint()
        {
            return new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
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

            await _udpClient.SendAsync(buffer, buffer.Length, endPoint);
        }

        private string GetAddressString(IPEndPoint endPoint)
        {
            return $"@udp://{endPoint.Address}:{endPoint.Port}";
        }
    }
}
