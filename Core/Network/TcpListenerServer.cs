using System;
using System.Net;
using System.Net.Sockets;

namespace Core.Network
{
    public class TcpListenerServer
    {
        private TcpListener listener;
        public Action<TcpClient> OnAcceptHandle;

        public void StartUp(int port, Action<TcpClient> onAcceptHandle)
        {
            if (onAcceptHandle == null)
            {
                throw new ArgumentNullException($"{nameof(onAcceptHandle)} is null !");
            }
            this.OnAcceptHandle = onAcceptHandle;
            listener = new TcpListener(IPAddress.Any,port);
            listener.Start();
            Debug.Log($"Run TcpListenerServer AcceptAsync !>>> addr:{IPAddress.Any},port:{port}");
            AcceptAsync();
        }

        private async void AcceptAsync()
        {
            var client = await listener.AcceptTcpClientAsync();
            if (client == null)
            {
                Debug.Log("TcpListenerServer Accept Error !");
                return;
            }
            else
            {
                OnAcceptHandle.Invoke(client);
                AcceptAsync();
            }
        }

        public void Stop()
        {
            listener.Stop();
        }
    }
}
