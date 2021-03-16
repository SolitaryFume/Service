using System;
using System.Net.Sockets;
using System.Linq;
using System.Threading;

namespace Service
{
    public interface IToken : IDisposable
    {
        public Guid ID{get;protected set;}
        public Socket Socket { get; protected set; }

        public void Init(Socket socket);
    }
}