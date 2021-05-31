using Core;
using Microsoft.Extensions.DependencyInjection;

namespace Core
{
    public class IOC
    {
        private static ServiceProvider _root;
        public static ServiceProvider Root
        {
            get => _root;
            set
            {
                Log.Assert(_root == null, "ServiceProvider Already Initialize !");
                _root = value;
            }
        }
    }
}