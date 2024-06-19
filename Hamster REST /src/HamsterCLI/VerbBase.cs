using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSRM.CS.DistributedSystems.Hamster
{
    internal abstract class VerbBase
    {
        public int Execute()
        {
            try
            {
                var hamsterManagement = new HamsterManagement();
                return ExecuteCore(hamsterManagement);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
                return 1;
            }
        }

        protected abstract int ExecuteCore(IHamsterManagement hamsterManagement);
    }
}
