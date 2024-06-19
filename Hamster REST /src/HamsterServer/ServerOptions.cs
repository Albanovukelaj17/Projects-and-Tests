using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSRM.CS.DistributedSystems.Hamster
{
    internal class ServerOptions
    {
        [Option('p', HelpText = "Port to run the server", Required = false, Default = (short)9000)]
        public short Port { get; set; }

        [Option('h', HelpText = "IP address to run the server on", Required = false, Default = "127.0.0.1")]
        public string IPAddress { get; set; }

        [Option('f', HelpText = "Sets the feed success probability", Required = false, Default = 0.0)]
        public double FeedSuccessProbability { get; set; } = 0;
    }
}
