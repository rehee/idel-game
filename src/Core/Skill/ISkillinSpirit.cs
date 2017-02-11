using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Skill
{
    public interface ISkillinSpirit
    {
        ISkillModel skill { get; set; }
        int CurrentCD { get; set; }
    }
}
