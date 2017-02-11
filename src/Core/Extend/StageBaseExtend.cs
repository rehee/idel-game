using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.Extend
{
    public static class StageBaseExtend
    {
        public static void AddMonster(this IStageBase stage, IMonsterBase monster)
        {
            try
            {
                if (stage.MonsterList == null)
                {
                    stage.MonsterList = new List<IMonsterBase>();
                }
                stage.MonsterList.Add(monster);
            }
            catch { }
        }
        public static void AddMonster(this IStageBase stage, IEnumerable<IMonsterBase> monsters)
        {
            try
            {
                if (stage.MonsterList == null)
                {
                    stage.MonsterList = new List<IMonsterBase>();
                }
                var result = monsters.Select(b => { stage.MonsterList.Add(b); return ""; }).ToList();
            }
            catch { }
        }
        public static void RemoveMonster(this IStageBase stage,int index)
        {
            try
            {
                if (stage.MonsterList == null)
                {
                    stage.MonsterList = new List<IMonsterBase>();
                    return;
                }
                stage.MonsterList.RemoveAt(index);
            }
            catch { }
        }
    }
}
