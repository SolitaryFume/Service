using System;
using Service;

namespace Service
{
    class Program
    {
        static void Main(string[] args)
        {
            // Console.WriteLine("Hello World!");
            // Services.GetService<TestService>().Log();
            Services.GetService<TcpService>().Start(1024);

            Console.ReadLine();
        }
    }
}

public static class Log
{
    public static void Print(string msg)
    {
        Console.WriteLine(msg);
    }

    internal static void Waring(string msg)
    {
        Console.WriteLine(msg);
    }
}
