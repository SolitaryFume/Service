using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Linq;
using System.Threading;

namespace Service
{
    public interface IToken : IDisposable
    {
        public Guid ID{get;protected set;}
        public Socket Socket { get; set;}

        public void Init(Socket socket);
    }
    
    public interface INet
    {
        public void SendMessage<T>(T message);
    }

    public class User : INet,IDisposable
    {
        public User()
        {
            _sendMessageQueue = new Queue<byte[]>();
            _receiveMessageList = new List<byte>();
        }

        private Socket _socket;
        private Queue<byte[]> _sendMessageQueue;
        private List<byte> _receiveMessageList;

        public void Dispose()
        {
            _sendMessageQueue.Clear();
            _receiveMessageList.Clear();
            _socket?.Close();
            _socket?.Dispose();
        }

        public void SendMessage<T>(T message)
        {
            if(message==null)
                return;
            var messageData = ProtoService.ProtoToMessage<T>(message);
            if(messageData==null)
                return;

            if(_sendMessageQueue.Count<=0)
            {
                //直接发送
                SendMessage(messageData);
            }
            else
            {
                _sendMessageQueue.Enqueue(messageData);
            }
        }

        private async void SendMessage(byte[] data)
        {
            if(_socket!=null)
            {
                var _ = await _socket.SendAsync(data,SocketFlags.None);
                if(_sendMessageQueue.Count>0)
                {
                    var messageData = _sendMessageQueue.Dequeue();
                    SendMessage(messageData);
                }
            }
            else
            {
                //客户端断开连接
                Dispose();
            }
        }

        private void OnReceiveData(byte[] data)
        {
            if(data==null || data.Length==0)    
                return;

            _receiveMessageList.AddRange(data);

            if(_receiveMessageList.Count>=8)
            {
                //尝试解析数据
            }   
        }
    }
}