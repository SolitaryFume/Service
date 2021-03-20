using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Service
{
    public interface IToken : IDisposable
    {
        public Guid ID{get;protected set;}
        public Socket Socket { get; set;}

        public void Init(Socket socket);
    }
    
    public interface ISession
    {
        public void SendMessage<T>(T message);
        public void MessageHander(Object message);
        public void StartReception();
        public void StopReception();
    }

    public interface IUser:ISession,IDisposable
    {
        
    }

    public class User : IUser
    {
        public User()
        {
            _sendMessageQueue = new Queue<byte[]>();
            _receiveMessageList = new List<byte>();
            readBuffer = new byte[1024];
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
            readTask?.Dispose();
        }
        public void MessageHander(Object message)
        {
            Debug.Log(System.Text.Json.JsonSerializer.Serialize(message));
            Services.GetService<MessageHanderService>().Dispatch(message,this);
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

        private int _messageSize = -1;
        private byte[] readBuffer;
        private void OnReceiveData(IEnumerable<byte> data)
        {
            if(data==null || data.Count()==0)    
                return;

            _receiveMessageList.AddRange(data);
            TryReadMessage();
        }

        private void TryReadMessage()
        {
            if(_receiveMessageList.Count<4) 
                return;
            
            if(_messageSize==-1) //设置读取消息长度
            {
                _messageSize = BitConverter.ToInt32(_receiveMessageList.Take(4).ToArray());
            }

            if(_receiveMessageList.Count>=_messageSize)
            {
                var message = ProtoService.MessageToProto(_receiveMessageList.Take(_messageSize).ToArray());
                MessageHander(message);
                TryReadMessage();
            }
        }

        private Task<int> readTask;
        public async void StartReception()
        {
            readTask = _socket.ReceiveAsync(readBuffer,SocketFlags.None);
            var count = await readTask;
            OnReceiveData(readBuffer.Take(count));
        }

        public void StopReception()
        {
            readTask?.Dispose();
        }
        
    }
}