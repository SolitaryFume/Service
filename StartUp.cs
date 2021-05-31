using Core.Network;
using MessageHander;
using Microsoft.Extensions.DependencyInjection;
using Proto;
using Core;
using Core.Client;

namespace Service
{
    public class StartUp
    {
        public StartUp()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            IOC.Root = services.BuildServiceProvider();

            IOC.Root.GetService<ITcpServer>().Start("0.0.0.0",8888);
        }

        private void ConfigureServices(ServiceCollection serviceCollection)
        {
            serviceCollection
                .AddSingleton<MessageHanderService>(new MessageHanderService())
                .AddSingleton<ITcpServer, TcpServer>()
                .AddTransient<ISession,TcpSession>()
                .AddTransient<IClient,ClientProxy>()
                ;
        }

        private void RegisterMessageHandler(ServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IMessageHander<LoginRequest>, LoginHnader>();
        }
    }
}