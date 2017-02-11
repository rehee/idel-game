using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace System.Extend
{
    public static class MonsterBaseExtend
    {
        public static void FillEmptyMonster(this IMonsterBase monsterFrom, IMonsterBase monsterTo)
        {
            try
            {
                monsterTo.Attribute.Attack = monsterFrom.Attribute.Attack;
                monsterTo.Status.AttackSpeed = monsterFrom.Status.AttackSpeed;
                monsterTo.Status.AttackCount = monsterFrom.Status.AttackCount;
                monsterTo.Experience = monsterFrom.Experience;
                monsterTo.Attribute.Hp = monsterFrom.Attribute.Hp;
                monsterTo.Attribute.MaxHp = monsterFrom.Attribute.MaxHp;
                monsterTo.Name = monsterFrom.Name;
            }
            catch { };
        }
        public static void EditMonster(this IMonsterBase monsterTarget, IMonsterBase monsterData)
        {
            try
            {
                monsterTarget.Attribute.Attack = monsterData.Attribute.Attack;
                monsterTarget.Status.AttackSpeed = monsterData.Status.AttackSpeed;
                monsterTarget.Status.AttackCount = monsterData.Status.AttackCount;
                monsterTarget.Experience = monsterData.Experience;
                monsterTarget.Attribute.Hp = monsterData.Attribute.Hp;
                monsterTarget.Attribute.MaxHp = monsterData.Attribute.Hp;
                monsterTarget.Name = monsterData.Name;
                monsterTarget.AddOrEditMonsterStageConfig(monsterData.StageConfig);
            }
            catch { };
        }
        public static void AddOrEditMonsterStageConfig(this IMonsterBase monster, Dictionary<int, int> stageConfig)
        {
            try
            {
                foreach(var item in stageConfig)
                {
                    monster.AddOrEditMonsterStageConfig(item.Key, item.Value);
                }
            }
            catch { }
             
        }
        public static void AddOrEditMonsterStageConfig(this IMonsterBase monster, int stageId, int number)
        {
            try
            {
                if (monster.StageConfig == null)
                    monster.StageConfig = new Dictionary<int, int>();
                if (monster.StageConfig.ContainsKey(stageId))
                {
                    monster.StageConfig[stageId] = number;
                }
                else
                {
                    monster.StageConfig.Add(stageId, number);
                }
            }
            catch { }
            
        }
        public static MonsterBaseJsonMode ToJsonMode(this IMonsterBase monster)
        {
            var jsonMode = new MonsterBaseJsonMode();
            if (monster == null)
                goto finish;
            try
            {
                jsonMode.name = monster.Name;
                jsonMode.hp = monster.Attribute.Hp.ToString();
                jsonMode.attack = monster.Attribute.Attack.ToString();
                jsonMode.exp = monster.Experience.ToString();
                jsonMode.maxHp= monster.Attribute.MaxHp.ToString();
            }
            catch { }

            finish:
            return jsonMode;
        }

    }
}
