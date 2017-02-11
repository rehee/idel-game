using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace System
{
    public interface IMonsterBase: ISpritBase
    {
        BigInteger Experience { get; set; }
        Dictionary<int,int> StageConfig { get; set; }
    }
}
