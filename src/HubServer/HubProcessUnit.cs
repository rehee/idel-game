using Core.Hubs;
using Core.Logics.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Service.ProcessService;
using System.Threading.Tasks;

namespace HubServer
{
    public class HubProcessUnit : IHubProcessUnit
    {
        public bool ProcessIng { get; private set; } = false;
        private bool ShowCombatLog { get; set; } = false;

        private IPlayerControl playerControl;
        
        public HubProcessUnit(IPlayerControl playerControl)
        {
            this.playerControl = playerControl;
        }

        public void StartProcess()
        {
            ProcessIng = true;
        }
        public void StopProcess()
        {
            ProcessIng = false;
        }
        public void DoCommand(List<string> command)
        {
            if (command == null || command.Count == 0)
                return;
            switch (command[0])
            {
                case "set":
                    SetUser(command);
                    break;
                case "quit":
                    QuitGame();
                    break;
                case "new":
                    NewChar(command);
                    break;
                case "f":
                    MoveForward();
                    break;
                case "b":
                    MoveBack();
                    break;
                case "show":
                    CombatLogSwitch();
                    break;
                case "a":
                    playerControl.AddCommand(command);
                    break;
                default:
                    return;
            }
        }
        public string PushMessage()
        {
            var status = playerControl.MyStatus();
            return status.ToJson();
        }

        public string PushMessageC2()
        {
            var status = playerControl.MyStatus();
            
            var combatLog = (CombatLog)status.combatLog.Log;
            var playerAttack = 0;
            if (combatLog.player.inAttack)
            {
                playerAttack = 1;
            }
            var monsterAttack = 0;
            if (combatLog.monster.inAttack)
            {
                monsterAttack = 1;
            }
            var C2Object = new
            {
                c2dictionary = true,
                data = new
                {
                    playerName = status.player.name,
                    playerHp = status.player.hp,
                    playerMaxHp = status.player.maxHp,
                    mapName = status.currentMap,
                    maxExp = status.player.nextExp,
                    currentExp = status.player.currentExp,
                    attack = playerAttack,
                    attackType = (int)combatLog.player.attackType,
                    monsterName = status.target.name,
                    monsterHp = status.target.hp,
                    monsterMaxHp = status.target.maxHp,
                    monsterAttack = monsterAttack,
                    playerAttacTime = status.combatLog.Time.Ticks,
                    monsterAttackTime = status.combatLog.Time.Ticks,
                    playerLevel = status.player.level,
                    attackName = combatLog.player.attackName,
                    skillName1 = status.player.skills[1].skillName,
                    skillCD1 = status.player.skills[1].cd,
                    skillMaxCD1 = status.player.skills[1].maxCd

                }
            };
            return C2Object.ToJson();
        }

        public string PushMessageActionLog()
        {
            try
            {
                var actions = playerControl.MyCharDetail.ActionLog
                    .OrderByDescending(b => b.Time)
                    .Select(b => $"{b.Item.ActionFrom} {b.Item.ActionType} {b.Item.ActionName} {b.Item.ActionTarget} {b.Item.Result}")
                    .ToList().ToJson();
                return actions;
            }
            catch
            {
                return "";
            }
        }


        private void SetUser(List<string> command)
        {
            if (command.Count < 2)
                return;
            int charId;
            var isInt = Int32.TryParse(command[1], out charId);
            if (!isInt)
                return;
            playerControl.SetChar(charId);
        }
        private void NewChar(List<string> command)
        {
            if (command.Count < 2)
                return;
            playerControl.CreateChar(command[1]);
        }
        private void MoveForward()
        {
            playerControl.MoveForward();
        }
        private void MoveBack()
        {
            playerControl.MoveBack();
        }
        private void CombatLogSwitch()
        {
            ShowCombatLog = !ShowCombatLog;
        }
        private void QuitGame()
        {
            ProcessIng = false;
        }
    }
}
