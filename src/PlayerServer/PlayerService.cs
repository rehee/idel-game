using Core.Service.PlayerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Service.GameService;
using System.Service.ProcessService;
using System.Threading.Tasks;

namespace PlayerServer
{
    public class PlayerService : IPlayerService
    {
        private IEnveroment env;
        private IGameService gameService;

        public PlayerService(IEnveroment env, IGameService gameService)
        {
            this.env = env;
            this.gameService = gameService;
        }

        private static List<IPlayerBase> playerList { get; set; } = new List<IPlayerBase>();

        public List<IPlayerBase> PlayerList { get { return playerList; } }

        public void CreateNewPlayer(out IPlayerBase player)
        {
            try
            {
                CreateNewPlayer("newplayer", out player);
            }
            catch { player = null; }

        }
        public void CreateNewPlayer(string name, out IPlayerBase player)
        {
            start:
            try
            {
                var newPlayer = env.NewPlayer();
                newPlayer.Name = name;
                var id = 1;
                var lastPlayer = PlayerList.OrderByDescending(b => b.Id).FirstOrDefault();
                if (lastPlayer != null)
                    id = lastPlayer.Id + 1;
                newPlayer.Id = id;
                PlayerList.Add(newPlayer);
                player = newPlayer;
                gameService.AddWorker(env.NewProcessService(player));
            }
            catch { goto start; }
        }

        public IPlayerBase GetPlayerById(int id)
        {
            start:
            try
            {
                return PlayerList.Where(b => b.Id == id).FirstOrDefault();
            }
            catch { goto start; }
            
        }
        public IProcessService GetProcessUnitByPlayerId(int id)
        {
            var count = 0;
            start:
            try
            {
                if (!gameService.Que.ContainsKey(id))
                    return null;
                return gameService.Que[id];
            }
            catch { count++;if (count < 10) goto start; return null; }
        }
    }
}
