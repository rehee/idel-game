using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.Service.StageService
{
    public interface IStageService
    {
        List<IStageBase> Stages { get; set; }
        List<IMonsterBase> MonsterPool { get; set; }
        List<IMonsterBase> MonsterInstancePool { get; set; }
        void AddStage(IStageBase stage);
        void AddStage(int id, string name);
        void AddStage(Dictionary<int,string> stages);
        IStageBase CreateStageBase(int id, string name);
        void GenerateStageMonster(int stageId);
    }
}
