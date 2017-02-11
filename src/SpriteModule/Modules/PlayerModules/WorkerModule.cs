using Core.Skill;
using SpriteModule.Modules;
using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace SpriteModule.Modules.PlayerModules
{
    public class WorkerModule : SpriteBase, IPlayerBase
    {
        public BigInteger WorkerExp { get; set; } = 0;
        public BigInteger NextExperience { get; set; } = 50;

        public FullCombatLog CombatLog { get; set; } = new FullCombatLog();
    }
}
