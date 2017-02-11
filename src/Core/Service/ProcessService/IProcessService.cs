using Core.Service.ProcessService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace System.Service.ProcessService
{
    public interface IProcessService : IProcessServiceBase
    {
        void AddCommand(List<string> command);
        IPlayerBase GetWorker();
        BigInteger GetExperience();

    }
}
