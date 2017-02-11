using Core.Skill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Numerics;

namespace SkillServer
{
    public class SkillModel : ISkillModel
    {
        public int SkillId { get; set; }
        public BigInteger Damage { get; set; }
        public string SkillName { get; set; }
        public int SkillCD { get; set; }
    }
}
