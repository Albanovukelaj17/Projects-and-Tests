using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HSRM.CS.DistributedSystems.Hamster
{
    internal abstract class VerbBase
    {
        [Option('h', HelpText = "The host name", Default = "localhost")]
        public string? Host { get; set; }

        [Option('p', HelpText = "The port number", Default = 9000)]
        public int Port { get; set; }

        public int Execute()
        {
            try
            {
                // avoid that .NET picks up the system proxy
                HttpClient.DefaultProxy = new WebProxy();
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"http://{Host}:{Port}");
                return ExecuteCore(client).Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return 1;
            }
        }

        protected abstract Task<int> ExecuteCore(HttpClient httpClient);
    }
}
