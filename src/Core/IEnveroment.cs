using System.Service.ProcessService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Service.StageService;
using Core.Hubs;
using Core.Logics.Control;
using Core.Service.PlayerBehaviourService;
using Core.Service.PlayerService;
using System.Service.GameService;
using Core.Skill;
using Core.Service.SkillService;
using Core.Service.ProcessService;

namespace System
{
    public interface IEnveroment
    {
        void SetStageService(IStageService stageService);
        void SetPlayerBehaviourService(IPlayerBehaviourService behaviourService);
        void SetPlayerService(IPlayerService playerService);
        void SetGameService(IGameService gameService);
        void SetSkillService(ISkillService skillService);

        IGameService ThisGameService { get; }

        IProcessService NewProcessService();
        IProcessService NewProcessService(IPlayerBase player);
        IProcessService ProcessServiceByPlayerId(int id);
        IMonsterBase NewMonster();
        IPlayerBase NewPlayer();
        IStageBase NewStage();
        IHubProcessUnit NewHubProcessUnit();
        IHubProcessUnit NewMonitorHubProcessUnit();
        IPlayerControl NewPlayerControl();
        IPlayerBehaviourService NewPlayerBehaviourService();
        ISkillModel NewSkillModel();
        ISkillinSpirit NewSkillinSpirit();
        IMonsterProcessService NewMonsterProcessService(IMonsterBase monster);
    }
}
