using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace Core.Logics.Manage
{
    public interface IMonsterManager
    {
        void ResetMonster();
        List<IMonsterBase> GetAllMonsterInPool();
        List<IMonsterBase> GetAllMonsterInStage(int stageId);
        List<IMonsterBase> GetAllMonsterInGame();

        IMonsterBase GetMonsterInPoolById(int monsterId);
        void AddMonsterInPool(IMonsterBase monster);
        void AddMonsterInPool(string monsterName,int attackSpeed,BigInteger Hp, BigInteger Attack ,BigInteger exp,out int monsterId);
        void RemoveMonsterInPool(int monsterId);
        
    }
}
