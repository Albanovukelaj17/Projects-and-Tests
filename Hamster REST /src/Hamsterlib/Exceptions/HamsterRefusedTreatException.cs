using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSRM.CS.DistributedSystems.Hamster.Exceptions
{
    public class HamsterRefusedTreatException : HamsterException
    {
        public HamsterRefusedTreatException()
            : base("The hamster refused the treat")
        {
        }
    }
}
