using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.Service.ProcessService
{
    public class ProcessServiceJsonMode
    {
        public PlayerBaseJsonMode player { get; set; } = new PlayerBaseJsonMode();
        public MonsterBaseJsonMode target { get; set; } = new MonsterBaseJsonMode();
        public string currentMap { get; set; } = "";
        public int currentMapId { get; set; } = -1;
        public FullCombatLog combatLog { get; set; } = null;

    }
}
