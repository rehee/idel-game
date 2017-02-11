using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Hubs
{
    public interface IHubProcessUnit
    {
        bool ProcessIng { get; }
        void DoCommand(List<string> command);
        string PushMessage();
        void StartProcess();
        void StopProcess();
        string PushMessageC2();
        string PushMessageActionLog();
    }
}
