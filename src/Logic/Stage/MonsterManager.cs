using Core.Logics.Manage;
using System;
using System.Collections.Generic;
using System.Extend;
using System.Linq;
using System.Service.StageService;
using System.Threading.Tasks;
using System.Numerics;

namespace Logic.Stage
{
    public class MonsterManager : IMonsterManager
    {
        private IEnveroment env;
        private IStageService stageService;

        public MonsterManager(IEnveroment env, IStageService stageService)
        {
            this.env = env;
            this.stageService = stageService;
        }

        public void AddMonsterInPool(IMonsterBase monster)
        {
            stageService.AddMonster(monster);
        }

        public List<IMonsterBase> GetAllMonsterInPool()
        {
            return this.stageService.MonsterPool;
        }

        public List<IMonsterBase> GetAllMonsterInGame()
        {
            try
            {
                return this.stageService.Stages.SelectMany(b => b.MonsterList).ToList();
            }
            catch { return new List<IMonsterBase>(); }
        }

        public void ResetMonster()
        {
            foreach(var item in stageService.Stages)
            {
                stageService.GenerateStageMonster(item.StageId);
            }
            
        }

        public List<IMonsterBase> GetAllMonsterInStage(int stageId)
        {
            try
            {
                return this.stageService.GetStageById(stageId).MonsterList;
            }
            catch { return null; }
            
        }

        public IMonsterBase GetMonsterInPoolById(int monsterId)
        {
            var count = 0;
            start:

            try
            {
                return stageService.MonsterPool.Where(b => b.Id == monsterId).FirstOrDefault();
            }
            catch { count++;if (count < 10) goto start;return null; }
        }

        public void AddMonsterInPool(string monsterName, int attackSpeed, BigInteger Hp, BigInteger Attack, BigInteger exp, out int monsterId)
        {
            try
            {
                var newMonster = env.NewMonster();
                newMonster.Name = monsterName;
                newMonster.Status.AttackSpeed = attackSpeed;
                newMonster.Attribute.Hp = Hp;
                newMonster.Attribute.Attack = Attack;
                newMonster.Experience = exp;
                newMonster.Attribute.MaxHp = Hp;
                stageService.AddMonster(newMonster);
                monsterId = newMonster.Id;
            }
            catch { monsterId = -1; }
            
        }

        public void RemoveMonsterInPool(int monsterId)
        {
            throw new NotImplementedException();
        }
    }
}
