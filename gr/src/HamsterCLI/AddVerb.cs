using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSRM.CS.DistributedSystems.Hamster
{
    [Verb("add", HelpText = "Adds a new hamster to the hamster institute")]
    internal class AddVerb : VerbBase
    {
        [Value(0, Required = true, HelpText = "The name of the owner")]
        public string OwnerName { get; set; }

        [Value(1, Required = true, HelpText = "The name of the hamster")]
        public string HamsterName { get; set; }

        [Value(2, Required = false, HelpText = "The number of initial treats", Default = (short)0)]
        public ushort Treats { get; set; }

        [Value(3, Required = false)]
        public IEnumerable<string> Remainder { get; set; } = Array.Empty<string>();

        protected override int ExecuteCore(IHamsterManagement hamsterManagement)
        {
            if (Remainder.Any())
            {
                return 2;
            }
            hamsterManagement.NewHamster(OwnerName, HamsterName, Treats);
            Console.WriteLine("Done!");
            return 0;
        }
    }
}
