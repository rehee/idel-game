using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System
{
    public interface IStageBase
    {
        int StageId { get; set; }
        string StageName { get; set; }
        List<IMonsterBase> MonsterList { get; set; }
    }
}
