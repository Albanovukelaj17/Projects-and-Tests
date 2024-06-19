using HSRM.CS.DistributedSystems.Hamster.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HSRM.CS.DistributedSystems.Hamster
{
    [Route("hamster")]
    [ApiController]
    public class HamsterController : ControllerBase
    {
        private IHamsterManagement lib;
        private ILogger<HamsterController> logger;

        public HamsterController(IHamsterManagement lib, ILogger<HamsterController> logger)
        {
            this.lib = lib;
            this.logger = logger;
        }

        private void LogHamsterIsSick(string hamster, string owner)
        {
            logger.LogWarning($"Attempted to feed sick hamster {hamster} of {owner}");
        }

        private void LogHamsterRefusedTreatsAfterRetry(string hamster, string owner)
        {
            logger.LogWarning($"Hamster {hamster} from {owner} has refused treats even after retry, we need a veterinarian.");
        }

        private void LogHamsterRecovered(string hamster, string owner)
        {
            logger.LogInformation($"Hamster {hamster} from {owner} has recovered, hurray!");
        }

        private void LogHamsterFeedAttemptAfterRest(string hamster, string owner)
        {
            logger.LogInformation($"Hamster {hamster} from {owner} had a rest for a while, maybe we can try another treat");
        }

        //TODO: Add endpoints here
    }
}
