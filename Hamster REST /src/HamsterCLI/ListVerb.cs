using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSRM.CS.DistributedSystems.Hamster
{
    [Verb("list", HelpText = "Lists the hamsters taken care of")]
    internal class ListVerb : VerbBase
    {
        [Value(0, Required = false, HelpText = "If set, restricts the output to hamsters of the given owner")]
        public string OwnerName { get; set; }

        [Value(1, Required = false)]
        public IEnumerable<string> Remainder { get; set; } = Array.Empty<string>();

        protected override int ExecuteCore(IHamsterManagement hamsterManagement)
        {
            if (Remainder.Any())
            {
                return 2;
            }
            var result = hamsterManagement.Search(OwnerName);
            if (result.Any())
            {
                Console.WriteLine("Owner\tName\tPrice\ttreats left");
                foreach (var hamsterId in result)
                {
                    var treats = hamsterManagement.ReadEntry(hamsterId, out var ownerName, out var hamsterName, out var price);
                    Console.WriteLine($"{ownerName}\t{hamsterName}\t{price} €\t{treats}");
                }
            }
            else
            {
                Console.WriteLine("No hamsters matching criteria found");
            }
            return 0;
        }
    }
}
