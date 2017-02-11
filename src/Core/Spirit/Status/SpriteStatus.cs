using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace Core.Spirit.Status
{
    public class SpriteStatus
    {
        public SpriteStatusType StatusType { get; set; } = SpriteStatusType.normal;

        public int SpawnTime { get; set; } = 50;
        public int SpawnCount { get; set; } = 0;

        public int AttackSpeed { get; set; } = 50;
        public int AttackCount { get; set; } = 0;

        public int FindTargetTime { get; set; } = 50;
        public int FintTargetCount { get; set; } = 0;



        public bool Attacking { get; set; } = false;
        public string AttackName { get; set; } = "";
        public BigInteger Damage { get; set; } = 0;
        public AttackTypeOption AttackType { get; set; } = AttackTypeOption.NormalAttack;
    }
}
