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
            var startup = new StartUp();
            Log.Print("服务服务启动成功!");
            Console.ReadLine();
        }
    }
}