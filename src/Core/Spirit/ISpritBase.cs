using Core.Skill;
using Core.Spirit.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace System
{
    public interface ISpritBase
    {
        int Id { get; set; }
        string Name { get; set; }
        int StageId { get; set; }

        SpriteAttribute Attribute { get; set; }
        SpriteStatus Status { get; set; }

        ISpritBase Target { get; set; }

        bool Active { get; set; }
        bool Distory { get; set; }

        Dictionary<int, DamageLog> DamageRecord { get; set; }
        List<FullActionLog> ActionLog { get; set; }
        Dictionary<int, ISkillinSpirit> SkillPool { get; set; }
    }



    
   


    
}
