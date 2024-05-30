using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSRM.CS.DistributedSystems.Hamster
{
    [Verb("feed", HelpText = "Feeds the given hamster")]
    internal class FeedVerb : VerbBase
    {
        [Value(0, Required = true, HelpText = "The name of the owner")]
        public string? OwnerName { get; set; }

        [Value(1, Required = true, HelpText = "The name of the hamster")]
        public string? HamsterName { get; set; }

        [Value(2, Required = true, HelpText = "The number of treats to give")]
        public short Treats { get; set; }

        [Value(3, Required = false)]
        public IEnumerable<string> Remainder { get; set; } = Array.Empty<string>();

        protected override int ExecuteCore(HamsterService.HamsterServiceClient hamsterManagement)
        {
            // todo: implement
            // Lookup the hamster's ID first
            var lookUpResponse =  hamsterManagement.LookUp(new HamsterLookUpRequest
            {
                OwnerName = OwnerName,
                HamsterName = HamsterName
            });

            // Give treats to the hamster
            var giveTreatsResponse =  hamsterManagement.GiveTreats(new HamsterGiveTreatsRequest
            {
                Id = lookUpResponse.Id,
                Treats = (ushort)Treats
            });
            Console.WriteLine(giveTreatsResponse.Treats);
            return 0; // Success
        }
    }
}
