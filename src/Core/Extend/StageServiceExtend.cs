using System.Service.StageService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace System.Extend
{
    public static class StageServiceExtend
    {
        public static int count { get; set; } = 0;
        public static void Init(this IStageService stageService, List<IStageBase> stages)
        {
            foreach (var item in stages)
            {
                stageService.AddStage(item);
            }
        }
        public static void AddMonster(this IStageService stageService,IMonsterBase monster)
        {
            start:
            try
            {
                if (stageService.MonsterPool == null)
                    stageService.MonsterPool = new List<IMonsterBase>();
                var lastMonster = stageService.MonsterPool.OrderByDescending(b => b.Id).FirstOrDefault();
                int id = 1;
                if (lastMonster != null)
                    id = lastMonster.Id + 1;
                monster.Id = id;
                stageService.MonsterPool.Add(monster);

            }
            catch { goto start; }
            
        }
        public static void AddMonster(this IStageService stageService, List<IMonsterBase> monsters)
        {
            try
            {
                var result = monsters.Select(b => { stageService.AddMonster(b); return ""; }).ToList();
            }
            catch { }
            
        }
        public static void GetMonster(this IStageService stageService, int id, IMonsterBase monster)
        {
            start:
            try
            {
                var random = new Random(System.DateTime.Now.Millisecond);
                var monsterList = stageService.GetStageById(id).MonsterList;
                int randomInt = random.Next(monsterList.Count);
                var item = monsterList[randomInt];
                item.FillEmptyMonster(monster);
            }
            catch { goto start;  }
        }
        public static ISpritBase GetMonster (this IStageService stageService, int id)
        {
            try
            {
                count++;
                if (count > 3000)
                {
                    count = 0;
                }

                var random = new Random(System.DateTime.Now.Millisecond + count);
                var monsterList = stageService.GetStageById(id).MonsterList.Where(b=>b.Attribute.Hp > 0).ToList();
                int randomInt = random.Next(monsterList.Count);
                var item = monsterList[randomInt];
                return (ISpritBase)item;
            }
            catch { return null; }
        }
        public static IStageBase GetStageById(this IStageService stageService, int id)
        {
            var count = 0;
            start:
            try
            {
                return stageService.Stages.Where(b => b.StageId == id).FirstOrDefault();
            }
            catch { count++;if (count < 100) goto start; return null; }
        }
        public static bool StageIdExist(this IStageService stageService,int stageId)
        {
            if (stageService.GetStageById(stageId) != null)
                return true;
            return false;
        }
    }
}
