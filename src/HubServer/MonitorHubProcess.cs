using Core.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Service.StageService;
using System.Threading.Tasks;

namespace HubServer
{
    public class MonitorHubProcess : IHubProcessUnit
    {
        private IStageService stageService;
        public MonitorHubProcess(IStageService stageService)
        {
            this.stageService = stageService;
        }
        public bool ProcessIng { get; private set; } = false;

        public void DoCommand(List<string> command)
        {

        }

        public string PushMessage()
        {
            var stage = new List<StageShortReport>();
            try
            {
                foreach (var item in stageService.Stages)
                {
                    var stageReport = new StageShortReport() { id = item.StageId, name = item.StageName };
                    try
                    {
                        foreach (var monster in item.MonsterList)
                        {
                            var monsterReport = new MonsterShortReport();
                            monsterReport.id = monster.Id;
                            monsterReport.hp = monster.Attribute.Hp.ToString();
                            monsterReport.name = monster.Name;
                            monsterReport.maxHp = monster.Attribute.MaxHp.ToString();
                            var target = monster.Target;
                            var targetName = "";
                            if (target != null)
                            {
                                targetName = target.Name;
                            }
                            monsterReport.currentTarget = targetName;
                            try
                            {
                                var topDamage = monster.DamageRecord
                                .OrderByDescending(b => b.Value.TotalDamage)
                                .Take(3)
                                .Select(b => {
                                    return new DamageTakenReport()
                                    {
                                        damage = b.Value.TotalDamage.ToString(),
                                        id = b.Key,
                                        name = b.Value.Character.Name
                                    };
                                })
                                .ToList();
                                monsterReport.damageTakens = topDamage;
                            }
                            catch { }
                            stageReport.monsters.Add(monsterReport);
                        }
                    }
                    catch { }
                    stage.Add(stageReport);
                }
            }
            catch { }
            
            return stage.ToJson();
        }
        public string PushMessageC2()
        {
            return "";
        }
        public string PushMessageActionLog()
        {
            return "";
        }



        public void StartProcess()
        {
            this.ProcessIng = true;
        }
        public void StopProcess()
        {
            this.ProcessIng = false;
        }
    }

    public class StageShortReport
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<MonsterShortReport> monsters { get; set; } = new List<MonsterShortReport>();
    }

    public class MonsterShortReport
    {
        public int id { get; set; }
        public string name { get; set; }
        public string hp { get; set; }
        public string maxHp { get; set; }
        public string currentTarget { get; set; }
        public List<DamageTakenReport> damageTakens { get; set; }
    }

    public class DamageTakenReport
    {
        public int id { get; set; }
        public string name { get; set; }
        public string damage { get; set; }
    }

}
