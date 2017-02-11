using Core.Service.ProcessService;
using Core.Spirit.Status;
using System;
using System.Collections.Generic;
using System.Extend;
using System.Linq;
using System.Threading.Tasks;

namespace MonsterServer
{
    public class MonsterProcessUnit : IMonsterProcessService
    {
        public Task ThisTask { get; set; } = null;
        private IMonsterBase monster;
        public MonsterProcessUnit(IMonsterBase monster)
        {
            this.monster = monster;
        }

        public int CurrentStage
        {
            get
            {
                if (monster == null)
                    return -1;
                return monster.StageId;
            }
        }

        public DateTime ProcessDateTime()
        {
            return DateTime.Now;
        }
        public ISpritBase GetMe()
        {
            return (ISpritBase)this.monster ?? null;
        }
        public ISpritBase GetTarget()
        {
            return (ISpritBase)monster.Target ?? null;
        }
        public void Target(ISpritBase target)
        {
            if (this.monster == null)
                return;
            this.monster.Target = target;
        }
        public void NextTick()
        {

            if (monster.Status.SpawnCount >= 0)
            {
                monster.Status.SpawnCount--;
                if (monster.Status.SpawnCount <= 0)
                {
                    try
                    {
                        monster.Attribute.Hp = monster.Attribute.MaxHp;
                        monster.Target = null;
                        monster.ClearDamageRecord();
                    }
                    catch { }
                }
                return;
            }
            if (monster.Attribute.Hp <= 0)
            {
                monster.Status.SpawnCount = monster.Status.SpawnTime;
                monster.Status.StatusType = SpriteStatusType.normal;
                return;
            }
            if (monster.Target == null)
            {
                monster.Status.StatusType = SpriteStatusType.normal;
                TargetTopThread();
                return;
            }
            
            if (monster.Attribute.Hp > 0|| monster.Target!=null)
            {
                if (monster.PlayerAttackCastFinish())
                {
                    monster.DoAttack(monster.Target);
                }
            }
            return;
        }


        private void TargetTopThread()
        {
            if(monster.DamageRecord==null|| monster.DamageRecord.Count == 0)
            {
                return;
            }
            try
            {
                var topThread = monster.DamageRecord.Select(b => b.Value).OrderByDescending(b => b.TotalDamage).FirstOrDefault();
                if (topThread == null)
                {
                    return;
                }
                if (topThread.Character.Attribute.Hp <= 0)
                {
                    Target(null);
                    return;
                }
                Target(topThread.Character);
            }
            catch { }
        }

    }
}
