using ChatCommon;
using System.Net;

namespace ChatApp
{
    public interface IClientMessageSource<T> where T : IPEndPoint
    {
        Task SendAsync(NetMessage msg, T endPoint);
        NetMessage Receive(ref T endPoint);
        T CreateEndPoint();
        T GetServer();
    }
}
