using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System
{
    public class PlayerBaseJsonMode
    {
        public int id { get; set; }
        public string name { get; set; }
        public int level { get; set; }
        public string maxHp { get; set; }
        public string hp { get; set; }
        public string attack { get; set; }
        public int attackSpeed { get; set; }
        public string currentExp { get; set; }
        public string nextExp { get; set; }
        public decimal petcentToNextLevel { get; set; }
        public string gapExp { get; set; }
        public bool inAttack { get; set; }
        public Dictionary<int, SkillLog> skills = new Dictionary<int, SkillLog>();
    }
    
    public class SkillLog
    {
        public string skillName { get; set; } = "";
        public int cd { get; set; } = 0;
        public int maxCd { get; set; } = 0;
    }
}
