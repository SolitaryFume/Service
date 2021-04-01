using System;
using Service;
using ProtoMessage;
using System.Text.Json;

namespace Service
{
    class Program
    {
        static void Main(string[] args)
        {
            // var login = new LoginRequest(){
            //     UserName = "UserName",
            //     Password = "Password"
            // };

            // var user = new User();
            // user.MessageHander(login);

            var dBServer = Services.GetService<DBServer>();
            dBServer.Init();

            Console.ReadLine();
        }

    }
}