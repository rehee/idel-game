using SpriteModule.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace SpriteModule.Modules.MonsterModules
{
    public class MonsterModel : SpriteBase, IMonsterBase
    {
        public BigInteger Experience { get; set; } = 1;
        public Dictionary<int, int> StageConfig { get; set; } = new Dictionary<int, int>();
    }
}
