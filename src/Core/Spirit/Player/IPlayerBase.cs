using Core.Skill;
using Core.Spirit.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace System
{
    public interface IPlayerBase : ISpritBase
    {
        BigInteger NextExperience { get; set; }
        BigInteger WorkerExp { get; set; }

        FullCombatLog CombatLog { get; set; }
    }


    

    public class FullCombatLog
    {
        public DateTime Time { get; } = DateTime.Now;
        public CombatLog Log { get; set; }
    }
    public class CombatLog
    {
        public PlayerLogs player { get; set; } = new PlayerLogs();
        public MonsterLog monster { get; set; } = new MonsterLog();
        public bool playerAttack { get; set; } = false;
        public bool monsterAttack { get; set; } = false;
    }


}
