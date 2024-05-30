using CommandLine;
using System;
using System.Collections.Generic;
using Grpc.Net.Client;

namespace HSRM.CS.DistributedSystems.Hamster
{
    [Verb("state", HelpText = "Queries the status of the given hamster")]
    internal class StateVerb : VerbBase
    {
        [Value(0, Required = true, HelpText = "The name of the owner")]
        public string OwnerName { get; set; }

        [Value(1, Required = true, HelpText = "The name of the hamster")]
        public string HamsterName { get; set; }

        [Value(2, Required = false, HelpText = "Additional arguments")]
        public IEnumerable<string> Remainder { get; set; } = Array.Empty<string>();

        protected override int ExecuteCore(HamsterService.HamsterServiceClient hamsterManagement)
        {
            
            
                var lookUpResponse = hamsterManagement.LookUp(new HamsterLookUpRequest
                {
                    OwnerName = OwnerName,
                    HamsterName = HamsterName
                });

                var howIsDoingResponse = hamsterManagement.HowIsDoing(new HamsterHowIsDoingRequest
                {
                    Id = lookUpResponse.Id
                });

                var hamsterState = howIsDoingResponse.Hamster;
                string message = $"{OwnerName}'s hamster {HamsterName} has done > {howIsDoingResponse.Hamster.Rounds} hamster wheel revolutions, and has {howIsDoingResponse.Hamster.Treats} treats left in store. Current price is {howIsDoingResponse.Hamster.Cost} €\n";
                Console.WriteLine(message);                

                return 0;
            }
            
        
    }
}