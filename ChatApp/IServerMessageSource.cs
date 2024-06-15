using ChatCommon;
using System.Net;

namespace ChatApp
{
    public interface IServerMessageSource<T> where T : IPEndPoint
    {
        Task SendAsync(NetMessage msg, T endPoint);
        NetMessage Receive(ref T endPoint);
        T CreateEndPoint();
        T CopyEndPoint(T endPoint);
    }
}
