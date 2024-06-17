using ChatCommon;
using System.Net;

namespace ChatApp.ClientMessageSource
{
    public interface IClientMessageSource<T>
    {
        Task SendAsync(NetMessage msg, T endPoint);
        NetMessage Receive(ref T endPoint);
        T CreateEndPoint();
        T GetServer();
    }
}
