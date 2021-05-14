using System;
using Service;
using ProtoMessage;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading;
using Core;
using Core.Network;

namespace Service
{
    class Program
    {
        static void Main(string[] args)
        {
            var tcplistener = new TcpListenerServer();
            tcplistener.StartUp(8888,tcp=> {
                Debug.Log("新的连接!");
                var session = new TcpSession();
                session.Initialize(tcp,bytes=> {
                    var message = System.Text.Encoding.UTF8.GetString(bytes);
                    Debug.Log(message);
                });
                var cts = new CancellationTokenSource();
                session.StartReceive(cts.Token);
                var data = System.Text.Encoding.UTF8.GetBytes("消息测试!");
                session.SendMessage(data);
            });

            Console.ReadLine();
        }
    }
}