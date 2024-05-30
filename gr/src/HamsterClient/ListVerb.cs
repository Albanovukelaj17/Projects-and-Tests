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
        [Option("owner", Required = false, HelpText = "If set, restricts the output to hamsters of the given owner")]
        public string? OwnerName { get; set; }

        [Option("hamster", Required = false, HelpText = "If set, restricts the output to hamsters with the given name")]
        public string? HamsterName { get; set; }

        [Value(1, Required = false)] public IEnumerable<string> Remainder { get; set; } = Array.Empty<string>();

        protected override int ExecuteCore(HamsterService.HamsterServiceClient hamsterManagement)
        {
                var request = new HamsterSearchRequest();
                
                if (!string.IsNullOrEmpty(OwnerName))
                {
                    request.OwnerName = OwnerName;
                }

                if (!string.IsNullOrEmpty(HamsterName))
                {
                    request.HamsterName = HamsterName;
                }

                var response = hamsterManagement.Search(request);
                Console.WriteLine(-1);
                foreach (var id in response.Id)
                {
                    var hamsterResponse = hamsterManagement.ReadEntry(new HamsterReadEntryRequest { Id = id });
                    Console.WriteLine($"{hamsterResponse.OwnerName}\t{hamsterResponse.HamsterName}\t{hamsterResponse.Price} €\t{hamsterResponse.Treats}");

                }
                return 0; // Success
            }
            
        }
    }

