using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Service;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading;
using Core;
using Core.Network;
using Proto;
using ProtoBuf;

namespace Service
{
    class Program
    {
        static void Main(string[] args)
        {
            var login = new LoginRequest()
            {
                Account = "账号",
                PassWorld = "密码",
            };

            var code = new MessageCode();

            var ls = new List<byte>();
            using (var steam = new MemoryStream())
            {
                Serializer.Serialize(steam,login);
                var o = steam.ToArray();
                var d = new byte[o.Length+4];
                Array.Copy(BitConverter.GetBytes(d.Length),0,d,0,2);
                Array.Copy(BitConverter.GetBytes(d.Length), 0, d, 2, 2);
                Array.Copy(o, 0, d, 4, o.Length);

                //code.Write(d, 0, d.Length);
                ls.AddRange(d);
                ls.AddRange(d);
                //for (int i = 0; i < ls.Count; i++)
                //{
                //    Debug.Log($"[{i}] = {ls[i]}");
                //    code.Write(new []{ls[i]},0,1);
                //}
                code.Write(ls.Take(3).ToArray(), 0, 3);
                code.Write(ls.Skip(3).ToArray(), 0, ls.Count-3);
            }
            var data = ProroHelp.Encoder(login);
            var msg = (LoginRequest)ProroHelp.Decoder(data);
            Debug.Log(msg.Account);

            //var code = new MessageCode();
            //code.Write(data,0,data.Length);

            Console.ReadLine();
        }
    }
}