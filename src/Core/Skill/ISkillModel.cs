using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace Core.Skill
{
    public interface ISkillModel
    {
        int SkillId { get; set; }
        string SkillName { get; set; }
        BigInteger Damage { get; set; }
        int SkillCD { get; set; } 
    }
}
