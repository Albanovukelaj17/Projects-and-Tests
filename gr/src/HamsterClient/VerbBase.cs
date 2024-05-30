using CommandLine;
using Grpc.Net.Client;
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

        [Option('p', HelpText = "The port number", Default = 2323)]
        public int Port { get; set; }

        public int Execute()
        {
            try
            {
                HttpClient.DefaultProxy = new WebProxy();
                var channel = GrpcChannel.ForAddress($"http://{Host}:{Port}");
                return ExecuteCore(new HamsterService.HamsterServiceClient(channel));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return 1;
            }
        }

        protected abstract int ExecuteCore(HamsterService.HamsterServiceClient hamsterManagement);
    }
}
