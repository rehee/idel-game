using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Logics.Manage
{
    public interface IStageManager
    {
        IList<IStageBase> GetAllStage();
        IStageBase GetStage(int stageId);
        List<IStageBase> GetStage(string stageName);
        void AddStage(string stageName,out int stageId);
        void ChangeStageName(int stageId, string stageName);
    }
}
