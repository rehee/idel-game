using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Service.PlayerBehaviourService
{
    public interface IPlayerBehaviourService
    {
        void MoveToStageById(int stageId);
        void SetCurrentPlayer(int id);
        IPlayerBase currentPlayer { get; }
    }
}
