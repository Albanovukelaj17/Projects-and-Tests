using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Reflection;
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

        [Value(1, Required = false)]
        public IEnumerable<string> Remainder { get; set; } = Array.Empty<string>();

        protected override async Task<int> ExecuteCore(HttpClient httpClient)
        {
            throw new NotImplementedException();
        }
    }
}
