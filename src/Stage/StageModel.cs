using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stage
{
    public class StageModel : IStageBase
    {
        public List<IMonsterBase> MonsterList { get; set; } = new List<IMonsterBase>();
        public int StageId { get; set; } = -1;
        public string StageName { get; set; } = "";
       
    }
}
