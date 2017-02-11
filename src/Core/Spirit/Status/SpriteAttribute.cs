using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace Core.Spirit.Status
{
    public class SpriteAttribute
    {
        public int Level { get; set; }
        public BigInteger Hp { get; set; }
        public BigInteger MaxHp { get; set; }
        public BigInteger Attack { get; set; }
    }
}
