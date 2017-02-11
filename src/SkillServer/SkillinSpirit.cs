using Core.Skill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillServer
{
    public class SkillinSpirit: ISkillinSpirit
    {

        public ISkillModel skill { get; set; }
        public int CurrentCD { get; set; } = 0;

        

    }
}
