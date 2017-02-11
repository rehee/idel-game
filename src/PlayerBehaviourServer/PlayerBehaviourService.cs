using Core.Service.PlayerBehaviourService;
using Core.Service.PlayerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Service.StageService;
using System.Threading.Tasks;
using System.Extend;
namespace PlayerBehaviourServer
{
    public class PlayerBehaviourService : IPlayerBehaviourService
    {
        private IStageService stageService;
        private IPlayerService playerService;
        public IPlayerBase currentPlayer { get; private set; }

        public PlayerBehaviourService(IStageService stageService, IPlayerService playerService)
        {
            this.stageService = stageService;
            this.playerService = playerService;
        }
        public void SetCurrentPlayer(int id)
        {
            currentPlayer = playerService.GetPlayerById(id);
        }
        public void MoveToStageById(int stageId)
        {
            if (currentPlayer == null)
                return;
            if (stageService.StageIdExist(stageId))
            {
                currentPlayer.StageId = stageId;
            }
            else { }
            
        }

        
    }
}
