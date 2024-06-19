using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSRM.CS.DistributedSystems.Hamster
{
    [Verb("bill", HelpText = "Shows the amount due for the given hamster owner")]
    internal class BillVerb : VerbBase
    {
        [Value(0, HelpText = "The owner of the hamsters that shall be billed", Required = true)]
        public string Owner { get; set; }

        [Value(1, Required = false)]
        public IEnumerable<string> Remainder { get; set; } = Array.Empty<string>();

        protected override int ExecuteCore(IHamsterManagement hamsterManagement)
        {
            if (Remainder.Any())
            {
                return 2;
            }
            if (Remainder.Any()) { return 2; }
            Console.WriteLine($"{Owner} has to pay {hamsterManagement.Collect(Owner)} €");
            return 0;
        }
    }
}
