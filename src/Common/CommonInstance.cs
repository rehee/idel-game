using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Service.ProcessService;
using System.Service.StageService;
using Core.Hubs;
using Core.Logics.Control;
using Core.Service.PlayerBehaviourService;
using Core.Service.PlayerService;
using System.Service.GameService;
using Core.Skill;
using Core.Service.SkillService;
using Core.Service.ProcessService;

namespace Common
{
    public class CommonInstance : IEnveroment
    {
        private Type iMonsterBaseType;
        private Type iPlayerBaseType;
        private Type iProcessServiceType;
        private Type iStageBaseType;
        private Type IHubProcessUnitType;
        private Type iPlayerControl;
        private Type iPlayerBehaviourService;
        private Type ISkillModelType;
        private Type ISkillinSpiritType;
        private Type IMonsterProcessServiceType;
        private Type MonitorHubProcessType;

        
        private IStageService stageService;
        private IPlayerBehaviourService behaviourService;
        private IPlayerService playerService;
        private IGameService gameService;
        private ISkillService skillService;

        public IGameService ThisGameService { get { return gameService; } }
        public CommonInstance(Type iMonsterBaseType, Type iPlayerBaseType, Type iProcessServiceType, Type iStageBaseType, Type IHubProcessUnitType, Type iPlayerControl, Type iPlayerBehaviourService, Type ISkillModelType, Type ISkillinSpiritType, Type IMonsterProcessServiceType ,Type MonitorHubProcessType)
        {
            this.iMonsterBaseType = iMonsterBaseType;
            this.iPlayerBaseType = iPlayerBaseType;
            this.iProcessServiceType = iProcessServiceType;
            this.iStageBaseType = iStageBaseType;
            this.IHubProcessUnitType = IHubProcessUnitType;
            this.iPlayerControl = iPlayerControl;
            this.iPlayerBehaviourService = iPlayerBehaviourService;
            this.ISkillModelType = ISkillModelType;
            this.ISkillinSpiritType = ISkillinSpiritType;
            this.IMonsterProcessServiceType = IMonsterProcessServiceType;
            this.MonitorHubProcessType = MonitorHubProcessType;
        }
        public void SetStageService(IStageService stageService)
        {
            this.stageService = stageService;
        }
        public void SetPlayerBehaviourService(IPlayerBehaviourService behaviourService)
        {
            this.behaviourService = behaviourService;
        }
        public void SetPlayerService(IPlayerService playerService)
        {
            this.playerService = playerService;
        }
        public void SetGameService(IGameService gameService)
        {
            this.gameService = gameService;
        }
        public void SetSkillService(ISkillService skillService)
        {
            this.skillService = skillService;
        }


        public IProcessService NewProcessService()
        {
            return (IProcessService)G.CreateGeneralType(iProcessServiceType);
        }
        public IProcessService NewProcessService(IPlayerBase player)
        {
            //var gateProcess = (IProcessService)G.CreateGeneralType(iProcessServiceType, player, stageService);
            var gateProcess = (IProcessService)G.CreateGeneralType(iProcessServiceType, player, stageService);
            return gateProcess;
        }
        public IProcessService ProcessServiceByPlayerId(int id)
        {
            try
            {
                return gameService.Que[id];
            }
            catch { return null; }
        }



        public IMonsterBase NewMonster()
        {
            return (IMonsterBase)G.CreateGeneralType(iMonsterBaseType);
        }

        public IPlayerBase NewPlayer()
        {
            var player = (IPlayerBase)G.CreateGeneralType(iPlayerBaseType);
            player.Name = "新玩家";
            player.Attribute.Attack = 1;
            player.Status.AttackSpeed = 50;
            player.Status.AttackCount = 0;
            player.Status.Damage = 1;
            player.Attribute.Hp = 100;
            player.Attribute.MaxHp = 100;
            player.Id = 1;
            player.Attribute.Level = 1;
            player.StageId = 1;
            player.NextExperience = 10;
            foreach (var item in skillService.SkillPool)
            {
                var index = skillService.SkillPool.IndexOf(item) + 1;
                if (index > 4)
                {
                    break;
                }
                var playerSkill = this.NewSkillInSpirit();
                playerSkill.SetSkill(item);
                if (!player.SkillPool.ContainsKey(index))
                {
                    continue;
                }
                player.SkillPool[index] = playerSkill;
            }
            return player;
        }

        public IStageBase NewStage()
        {
            return (IStageBase)G.CreateGeneralType(iStageBaseType);
        }

        public IPlayerControl NewPlayerControl()
        {
            var playerControl = (IPlayerControl)G.CreateGeneralType(iPlayerControl, this.NewPlayerBehaviourService(), playerService, stageService, this);
            return playerControl;
        }

        public IPlayerBehaviourService NewPlayerBehaviourService()
        {
            return (IPlayerBehaviourService)G.CreateGeneralType(iPlayerBehaviourService, stageService, playerService);
        }

        public IHubProcessUnit NewHubProcessUnit()
        {
            return (IHubProcessUnit)G.CreateGeneralType(IHubProcessUnitType, this.NewPlayerControl());
        }
        public IHubProcessUnit NewMonitorHubProcessUnit()
        {
            return (IHubProcessUnit)G.CreateGeneralType(MonitorHubProcessType, this.stageService);
        }

        public ISkillModel NewSkillModel()
        {
            return (ISkillModel)G.CreateGeneralType(ISkillModelType);
        }

        public ISkillinSpirit NewSkillInSpirit()
        {
            return (ISkillinSpirit)G.CreateGeneralType(ISkillinSpiritType);
        }

        public ISkillinSpirit NewSkillinSpirit()
        {
            return NewSkillInSpirit();
        }
        public IMonsterProcessService NewMonsterProcessService(IMonsterBase monster)
        {
            return (IMonsterProcessService)G.CreateGeneralType(IMonsterProcessServiceType, monster);
        }
    }
}
