using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSRM.CS.DistributedSystems.Hamster
{
    [Verb("state", HelpText = "Queries the status of the given hamster")]
    internal class StateVerb : VerbBase
    {
        [Value(0, Required = true, HelpText = "The name of the owner")]
        public string OwnerName { get; set; }

        [Value(1, Required = true, HelpText = "The name of the hamster")]
        public string HamsterName { get; set; }

        [Value(2, Required = false)]
        public IEnumerable<string> Remainder { get; set; } = Array.Empty<string>();

        protected override int ExecuteCore(IHamsterManagement hamsterManagement)
        {
            if (Remainder.Any())
            {
                return 2;
            }
            var state = hamsterManagement.Howsdoing(hamsterManagement.Lookup(OwnerName, HamsterName));
            Console.WriteLine($"{OwnerName}'s hamster {HamsterName} has done > {state.Rounds} hamster wheel revolutions,\r\nand has {state.TreatsLeft} treats left in store. Current price is {state.Cost} €");
            return 0;
        }
    }
}
