using Core.Network;
using MessageHander;
using Microsoft.Extensions.DependencyInjection;
using Proto;

namespace Service
{
    public class StartUp
    {
        public StartUp()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            IOC.Root = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection serviceCollection)
        {
            serviceCollection
                .AddSingleton<ITcpServer, TcpServer>();
        }

        private void RegisterMessageHandler(ServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IMessageHander<LoginRequest>, LoginHnader>();
        }
    }
}