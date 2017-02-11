using System;
using System.Collections.Generic;
using System.Linq;
using System.Service.ProcessService;
using System.Threading.Tasks;

namespace Core.Service.PlayerService
{
    public interface IPlayerService
    {
        List<IPlayerBase> PlayerList { get; }
        void CreateNewPlayer(out IPlayerBase player);
        void CreateNewPlayer(string name,out IPlayerBase player);

        IPlayerBase GetPlayerById(int id);
        IProcessService GetProcessUnitByPlayerId(int id);

    }
}
