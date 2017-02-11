using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Skill
{
    public static class SkillExtend
    {
        public static void SetSkill(this ISkillinSpirit skillInPlayer, ISkillModel skill)
        {
            try
            {
                skillInPlayer.skill = skill;
            }
            catch { }
        }
    }
}
