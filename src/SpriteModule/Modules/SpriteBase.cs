using Core.Skill;
using Core.Spirit.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace SpriteModule.Modules
{
    public abstract class SpriteBase : ISpritBase
    {
        public int Id { get; set; } = -1;
        public string Name { get; set; } = "";
        public int StageId { get; set; } = -1;

        public SpriteAttribute Attribute { get; set; } = new SpriteAttribute();
        public SpriteStatus Status { get; set; } = new SpriteStatus();

        public ISpritBase Target { get; set; } = null;

        public bool Active { get; set; } = true;
        public bool Distory { get; set; } = false;

        public Dictionary<int, DamageLog> DamageRecord { get; set; } = new Dictionary<int, DamageLog>();
        public List<FullActionLog> ActionLog { get; set; } = new List<FullActionLog>();
        public Dictionary<int, ISkillinSpirit> SkillPool { get; set; } = new Dictionary<int, ISkillinSpirit>()
        {
            [1] = null,
            [2] = null,
            [3] = null,
            [4] = null
        };
    }
}
