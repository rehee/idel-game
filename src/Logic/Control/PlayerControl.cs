using Core.Logics.Control;
using Core.Service.PlayerBehaviourService;
using Core.Service.PlayerService;
using Logic.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Service.StageService;
using System.Threading.Tasks;
using System.Extend;
using System.Service.ProcessService;

namespace Logic.Control
{
    public class PlayerControl : IPlayerControl
    {
        private IPlayerBehaviourService behaviourService;
        private IPlayerService playerService;
        private IStageService stageService;
        private IEnveroment env;
        public IPlayerBase MyCharDetail { get; private set; }
        public IStageBase GetCurrentMap(int id)
        {
            return stageService.GetStageById(id);
        }
        public IStageBase CurrentMap
        {
            get
            {
                try
                {
                    return stageService.GetStageById(MyCharDetail.StageId);
                }
                catch
                {
                    var emptyStage = env.NewStage();
                    emptyStage.StageName = "";
                    return emptyStage;
                }
            }
        }

        public PlayerControl(IPlayerBehaviourService behaviourService, IPlayerService playerService, IStageService stageService, IEnveroment env)
        {
            this.behaviourService = behaviourService;
            this.playerService = playerService;
            this.stageService = stageService;
            var player = playerService.PlayerList.FirstOrDefault();
            if(player != null)
            {
                this.MyCharDetail = player;
            }else
            {
                this.MyCharDetail = env.NewPlayer();
            }
            this.env = env;
        }

        public void CreateChar(string name)
        {
            IPlayerBase newChar;
            this.playerService.CreateNewPlayer(name, out newChar);
            if (newChar != null)
                SetChar(newChar.Id);
        }
        public void MoveBack()
        {
            MoveStage(forward: false);
        }
        public void MoveForward()
        {
            MoveStage();
        }
        public void SetChar(int playerId)
        {
            behaviourService.SetCurrentPlayer(playerId);
            MyCharDetail = behaviourService.currentPlayer;
        }
        public ProcessServiceJsonMode MyStatus()
        {
            if (MyCharDetail == null)
                return null;
            var jsonMode = playerService.GetProcessUnitByPlayerId(MyCharDetail.Id).ToJsonMode();
            jsonMode.currentMap = GetCurrentMap(MyCharDetail.StageId).StageName;
            return jsonMode;
        }
        public void AddCommand(List<string> command)
        {
            if (command == null && command.Count <= 0)
                return;
            if (command[0] != "a")
                return;
            IProcessService process=null;
            try
            {
                process = env.ProcessServiceByPlayerId(MyCharDetail.Id);

            }
            catch { }
            if (process == null)
                return;
            process.AddCommand(command);
            
        }


        private void MoveStage(int step = 1, bool forward = true)
        {
            if (MyCharDetail == null)
                return;
            var currentId = MyCharDetail.StageId;

            var mapIds = stageService.Stages.OrderBy(b=>b.StageId).Select(b => b.StageId).ToList();
            var index = mapIds.IndexOf(currentId);
            if (index == -1)
                return;
            if (forward)
            {
                currentId = index + step;
            }
            else
            {
                currentId = index - step;
            }
            try
            {
                behaviourService.SetCurrentPlayer(MyCharDetail.Id);
                behaviourService.MoveToStageById(mapIds[currentId]);
            }
            catch { }

            
        }



    }
}
