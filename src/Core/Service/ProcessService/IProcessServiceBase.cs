using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Service.ProcessService
{
    public interface IProcessServiceBase
    {
        int CurrentStage { get; }
        void NextTick();
        DateTime ProcessDateTime();
        ISpritBase GetTarget();
        ISpritBase GetMe();
        void Target(ISpritBase target);

        Task ThisTask { get; set; }
    }
}
