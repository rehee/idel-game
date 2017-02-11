using System;
using System.Service.StageService;
using System.Extend;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Service.ProcessService;
using System.Service.GameService;

namespace StageServer
{
    public class StageService : IStageService
    {
        private IEnveroment env;
        private const int monsterTick = 100;
        

        public StageService(IEnveroment env)
        {
            this.env = env;
            MonsterLogicThread();
        }

        public List<IStageBase> Stages { get; set; } = new List<IStageBase>();
        public List<IMonsterBase> MonsterPool { get; set; } = new List<IMonsterBase>();
        public List<IMonsterBase> MonsterInstancePool { get; set; } = new List<IMonsterBase>();
        private Dictionary<int, IMonsterProcessService> MonsterProcessPool { get; set; } = new Dictionary<int, IMonsterProcessService>();
        public void AddStage(IStageBase stage)
        {
            try
            {
                int id = 1;
                var last = this.Stages.OrderByDescending(b => b.StageId).FirstOrDefault();
                if (last != null)
                {
                    id = last.StageId + 1;
                }
                stage.StageId = id;
                this.Stages.Add(stage);
            }
            catch { }

        }
        public void AddStage(int id = -1, string name = "无名地图")
        {
            try
            {
                var checkDupe = this.Stages.Where(c => c.StageId == id || c.StageName == name).ToList().Count > 0;
                if (checkDupe)
                {
                    var last = this.Stages.OrderByDescending(b => b.StageId).FirstOrDefault();
                    id = last.StageId + 1;
                }

                var newStage = env.NewStage();
                newStage.StageId = id;
                newStage.StageName = name;
                this.AddStage(newStage);
            }
            catch { };
        }
        public void AddStage(Dictionary<int, string> stages)
        {
            var result = stages.Select(b => { this.AddStage(b.Key, b.Value); return ""; }).ToList();
        }
        public IStageBase CreateStageBase(int id, string name)
        {
            try
            {
                this.AddStage(id, name);
                return this.Stages.Where(b => b.StageId == id).FirstOrDefault();
            }
            catch
            {
                return null;
            }

        }
        public void GenerateStageMonster(int stageId)
        {
            try
            {
                var stage = this.GetStageById(stageId);
               
                var monsterList = new List<IMonsterBase>();
                foreach (var item in MonsterPool)
                {
                    try
                    {
                        int count = item.StageConfig[stageId];
                        for (var i = 0; i < count; i++)
                        {
                            var monster = env.NewMonster();
                            item.FillEmptyMonster(monster);
                            monster.StageId = stageId;
                            var lastMonsterInstance = MonsterInstancePool.OrderByDescending(b => b.Id).FirstOrDefault();
                            if (lastMonsterInstance == null)
                            {
                                monster.Id = 1;
                            }
                            else
                            {
                                monster.Id = lastMonsterInstance.Id + 1;
                            }
                            monster.Active = true;
                            MonsterInstancePool.Add(monster);
                            monsterList.Add(monster);
                            StartMonsterProcessUnit(monster);
                        }
                    }
                    catch { continue; }
                }

                foreach (var monster in stage.MonsterList)
                {
                    MonsterProcessPool.Remove(monster.Id);
                    MonsterInstancePool.Remove(monster);
                }
                stage.MonsterList = monsterList;
            }
            catch { }
        }


        private void StartMonsterProcessUnit(IMonsterBase monster)
        {
            try
            {
                if (MonsterProcessPool.ContainsKey(monster.Id))
                {
                    return;
                }
                var monsterProcessUnit = env.NewMonsterProcessService(monster);
                MonsterProcessPool.Add(monster.Id, monsterProcessUnit);
            }
            catch { }
            
        }

        private void CreateMonsterProcessUnitTask(IMonsterProcessService monsterProcessUnit)
        {
            Task monsterUnit = new Task(() =>
            {
                

                Console.WriteLine("walalalala I am dead");
            });
            monsterProcessUnit.ThisTask = monsterUnit;
            monsterUnit.Start();
           
        }

        private void MonsterLogicThread()
        {
            Task stageProcess = new Task(()=> {
                while (true)
                {
                    System.Threading.Thread.Sleep(monsterTick);
                    try
                    {
                        int count = Stages.Count;
                        for(var i = 0; i < count; i++)
                        {
                            ProcessMonstarLogic(Stages[i]);
                        }
                    }
                    catch { }

                }
            });
            stageProcess.Start();
        }

        private void ProcessMonstarLogic(IStageBase stage)
        {
            try
            {
                if (env.ThisGameService.Que.Where(b => b.Value.CurrentStage == stage.StageId).Count() <= 0)
                {
                    return;
                }
            }
            catch { }
            try
            {
                var count = stage.MonsterList.Count;
                for (var i = 0; i < count; i++)
                {
                    MonsterProcessPool[stage.MonsterList[i].Id].NextTick();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //Task union = new Task(() =>
            //{



            //});
            //union.Start();

        }


    }
}
