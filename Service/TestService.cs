using System;
using System.Threading.Tasks;

namespace Service
{
    public class TestService : ServiceBase
    {
        private TestService()
        {

        }

        public void Log()
        {
            Console.WriteLine("TestService log !");
        }
    }
}