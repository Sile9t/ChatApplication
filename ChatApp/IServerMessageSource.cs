using ChatCommon;
using System.Net;

namespace ChatApp
{
    public interface IServerMessageSource<T>
    {
        Task SendAsync(NetMessage msg, T endPoint);
        NetMessage Receive(ref T endPoint);
        T CreateEndPoint();
        T CopyEndPoint(T endPoint);
    }
}
