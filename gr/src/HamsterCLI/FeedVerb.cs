﻿using CommandLine;
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
        public string OwnerName { get; set; }

        [Value(1, Required = true, HelpText = "The name of the hamster")]
        public string HamsterName { get; set; }

        [Value(2, Required = true, HelpText = "The number of treats to give")]
        public ushort Treats { get; set; }

        [Value(3, Required = false)]
        public IEnumerable<string> Remainder { get; set; } = Array.Empty<string>();

        protected override int ExecuteCore(IHamsterManagement hamsterManagement)
        {
            if (Remainder.Any())
            {
                return 2;
            }
            var remaining = hamsterManagement.GiveTreats(hamsterManagement.Lookup(OwnerName, HamsterName), Treats);
            Console.WriteLine($"Done! {remaining} treats remaining in store");
            return 0;
        }
    }
}
