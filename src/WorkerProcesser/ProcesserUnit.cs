using System;
using System.Extend;
using System.Service.ProcessService;
using System.Service.StageService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Core.Spirit.Logs;
using Core.Skill;
using Core.Spirit.Status;

namespace WorkerProcesser
{
    public class ProcesserUnit : IProcessService
    {
        private const int CommandPollLimit = 5;
        private IPlayerBase player;
        private IStageService stageService;
        private List<List<string>> commandPoll = new List<List<string>>();
        public ProcesserUnit(IPlayerBase player, IStageService stageService)
        {
            this.player = player;
            this.stageService = stageService;
        }
        public ISpritBase GetMe()
        {
            return (ISpritBase)this.player ?? null;
        }
        public void Target(ISpritBase target)
        {
            player.Target = target;
        }
        public int CurrentStage
        {
            get
            {
                try
                {
                    return this.player.StageId;
                }
                catch { return -1; }
            }
        }
        public DateTime ProcessDateTime()
        {
            return DateTime.Now;
        }
        public IPlayerBase GetWorker()
        {
            return player;
        }
        public ISpritBase GetTarget()
        {
            return player.Target;
        }
        public void AddCommand(List<string> command)
        {
            if (command == null || command.Count <= 0)
                return;
            try
            {
                if (commandPoll.Count > 5)
                {
                    return;
                }
            }
            catch { return; }
            PreProcessCommand(command);
        }
        public void NextTick()
        {
            ProcessSkillCD(player.SkillPool);
            CheckPlayerDeathAndSpawn();
            TargetSelect();
            ProcessCommandAndAttack();
            CheckMonsterDeadAndEXP();
            PrepareCombatLog();
        }
        public BigInteger GetExperience()
        {
            return player.WorkerExp;
        }
        private void CheckMonsterDeadAndEXP()
        {
            if (player.Target != null && player.Target.Attribute.Hp <= 0)
            {
                player.AddExperience(((IMonsterBase)player.Target).Experience);
            }
        }
        private void PrepareCombatLog()
        {
            var log = CombatLog();
            if (log != null)
                player.CombatLog = new FullCombatLog() { Log = log };
        }
        private void CheckPlayerDeathAndSpawn()
        {
            if (player.Attribute.Hp <= 0 && player.Status.SpawnCount <= 0)
            {
                player.Status.SpawnCount = player.Status.SpawnTime;
                return;
            }
            if (player.Status.SpawnCount <= 0)
            {
                return;
            }
            player.Status.SpawnCount--;
            if (player.Status.SpawnCount <= 0)
            {
                player.Status.StatusType = SpriteStatusType.normal;
                player.Attribute.Hp = player.Attribute.MaxHp;
            }
        }

        private void TargetSelect()
        {
            if (player.Target != null && player.Target.Attribute.Hp > 0)
            {
                return;
            }
            player.Target = null;
            player.Status.StatusType = SpriteStatusType.normal;
            if (player.Status.FintTargetCount > 0)
            {
                player.Status.FintTargetCount--;
                return;
            }
            var monster = stageService.GetMonster(player.StageId);
            if (monster == null)
            {
                return;
            }
            if (monster.Attribute.Hp <= 0)
            {
                player.Target = null;
                return;
            }
            player.Target = monster;
            player.Status.FintTargetCount = player.Status.FindTargetTime;

        }

        private CombatLog CombatLog()
        {
            IMonsterBase monster = null;
            if (player != null)
            {
                try
                {
                    monster = (IMonsterBase)player.Target;
                }
                catch { }

            }

            bool isPlayerCombat = false;
            bool isMonsterCombat = false;
            if (player != null && player.Status.Attacking)
            {
                isPlayerCombat = true;
            }
            if (monster != null && monster.Status.Attacking)
            {
                isMonsterCombat = true;
            }

            if (isPlayerCombat == false && isMonsterCombat == false)
                return null;


            PlayerLogs playerLog = new PlayerLogs();
            MonsterLog monsterLog = new MonsterLog();
            var result = new CombatLog();
            result.playerAttack = isPlayerCombat;
            result.monsterAttack = isMonsterCombat;
            if (player != null)
            {
                playerLog = new PlayerLogs()
                {
                    level = player.Attribute.Level,
                    hp = player.Attribute.Hp.ToString(),
                    exp = player.WorkerExp.ToString(),
                    next = player.NextExperience.ToString(),
                    inAttack = isPlayerCombat,
                    damage = player.Status.Damage.ToString(),
                    attackName = player.Status.AttackName,
                    attackType = player.Status.AttackType
                };
            }
            monsterLog = new MonsterLog();
            if (monster != null)
            {

                monsterLog.name = monster.Name;
                monsterLog.attack = monster.Attribute.Attack.ToString();
                monsterLog.exp = monster.Experience.ToString();
                monsterLog.hp = monster.Attribute.Hp.ToString();
                monsterLog.damage = monster.Status.Damage.ToString();
                monsterLog.inAttack = isMonsterCombat;
            }
            result.player = playerLog;
            result.monster = monsterLog;
            return result;
        }
        private void ProcessCommandAndAttack()
        {
            if (player.Attribute.Hp <= 0)
            {
                commandPoll.Clear();
                return;
            }
            var commandTotal = commandPoll.Count;
            try
            {
                if (commandTotal > 0)
                {
                    ProcessCommand();
                    return;
                }
            }
            catch
            {
                try { commandPoll.RemoveAt(0); } catch { }
            }
            if (player.PlayerAttackCastFinish())
            {
                player.DoAttack(player.Target);
            };
        }


        private void PreProcessCommand(List<string> command)
        {
            switch (command[0])
            {
                case "a":
                    PreProcessAttackCommand(command);
                    break;
                default:
                    break;
            }
        }
        private void PreProcessAttackCommand(List<string> command)
        {
            if (command.Count == 1)
            {
                CommandPoolAddByLimit(command);
                return;
            }
            int skillId;
            Int32.TryParse(command[1], out skillId);
            if (skillId <= 0)
                return;
            try
            {
                if (!player.SkillPool.ContainsKey(skillId))
                {
                    return;
                }
                if (player.SkillPool[skillId].CurrentCD > 0)
                {
                    return;
                }
                CommandPoolAddByLimit(command);
            }
            catch { }

        }

        private void ProcessCommand()
        {

            if (commandPoll.Count < 1)
                return;
            List<string> command = commandPoll[0];
            switch (command[0])
            {
                case "a":
                    ProcessAttackCommand(command);
                    break;
                default:
                    break;
            }
            commandPoll.RemoveAt(0);
        }
        private void ProcessAttackCommand(List<string> command)
        {
            if (command.Count <= 1)
            {
                string attackName = "主动攻击";
                player.DoAttack(player.Target, attackName);
            }
            else
            {
                int skillIndex;
                if (!int.TryParse(command[1], out skillIndex))
                {
                    return;
                }
                if (!player.SkillPool.ContainsKey(skillIndex))
                {
                    return;
                }
                var skill = player.SkillPool[skillIndex];
                if (skill == null || skill.CurrentCD > 0)
                {
                    return;
                }
                player.DoAttack(player.Target, "", skill);
            }
        }
        private void ProcessSkillCD(Dictionary<int, ISkillinSpirit> skillPool)
        {
            foreach (var item in skillPool)
            {
                if (item.Value == null)
                {
                    continue;
                }
                try
                {
                    if (item.Value.CurrentCD > 0)
                    {
                        item.Value.CurrentCD = item.Value.CurrentCD - 1;
                    }
                }
                catch { }
            }
        }

        private void CommandPoolAddByLimit(List<string> command, int limit = CommandPollLimit)
        {
            commandPoll.AddLimit(command, limit);
        }
        public Task ThisTask { get; set; } = null;
    }


}
