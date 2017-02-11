using System.Service.ProcessService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.Service.GameService
{
    public interface IGameService
    {
        void AddWorker(IProcessService worker);
        Dictionary<int, IProcessService> Que { get; }
    }
}
