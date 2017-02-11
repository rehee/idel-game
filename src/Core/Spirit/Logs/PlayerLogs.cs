using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Spirit.Logs
{
    public class PlayerLogs
    {
        public int level { get; set; }
        public string hp { get; set; }
        public string exp { get; set; }
        public string next { get; set; }
        public bool inAttack { get; set; }
        public string damage { get; set; }
        public string attackName { get; set; }
        public AttackTypeOption attackType { get; set; } = AttackTypeOption.NormalAttack;
    }
    

    public class MonsterLog
    {
        public int level { get; set; }
        public string name { get; set; }
        public string attack { get; set; }
        public string hp { get; set; }
        public string exp { get; set; }
        public string next { get; set; }
        public bool inAttack { get; set; }
        public string damage { get; set; }
        public string attackName { get; set; }
    }
}
