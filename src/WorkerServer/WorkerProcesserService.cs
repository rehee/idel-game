using System;
using System.Extend;
using System.Service.GameService;
using System.Service.ProcessService;
using System.Service.StageService;
using Stage;
using StageServer;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkerProcesser;

namespace WorkerServer
{
    public class WorkerProcesserService : IGameService
    {
        
        private const int playerTick = 100;
        private const int ProcessThread = 2;
        private static Dictionary<int, IProcessService> Workers { get; set; } = new Dictionary<int, IProcessService>();
        private static List<IProcessService> processPool { get; set; } = new List<IProcessService>();
        private IStageService StageService;
        private IEnveroment env;


        public Dictionary<int, IProcessService> Que
        {
            get
            {
                return Workers;
            }
        }

        public WorkerProcesserService(IStageService stageService, IEnveroment env)
        {
            this.StageService = stageService;
            this.env = env;
            GetProcessTask();
        }

        public void AddWorker(IProcessService worker)
        {
            try
            {
                Que.Add(worker.GetWorker().Id, worker);
                processPool.Add(worker);
            }
            catch { }
        }

        private void GetProcessTask()
        {

            for (var i = 1; i < ProcessThread + 1; i++)
            {
                ProcessPlayerUnit(i);
            }
        }
        private void GetCurrectMonsterNotNull(IProcessService union, int stageId, IMonsterBase monster, bool generateNewMonster = true)
        {
            if (generateNewMonster)
                StageService.GetMonster(union.GetWorker().StageId, monster);
            while (monster == null)
            {
                StageService.GetMonster(union.GetWorker().StageId, monster);
                System.Threading.Thread.Sleep(100);
            }
        }

        private void ProcessPlayerUnit(int threadCount)
        {
            var processCount = new ProcessThreadCount(threadCount, ProcessThread);
            Task thd = new Task((ProcessThreadCount) =>
            {
                while (true)
                {
                    Thread.Sleep(playerTick);
                    var total = processPool.Count;
                    var multi = total / processCount.totalThread;
                    var mod = total % processCount.totalThread;
                    var index = processCount.currentThread - 1;
                    var start = index * multi;
                    var finish = processCount.currentThread * multi;
                    if (multi == 0)
                    {
                        finish = finish + mod;
                    }
                    if (processCount.currentThread > total)
                    {
                        continue;
                    }
                    if (finish > total)
                    {
                        finish = total;
                    }
                    if (processCount.currentThread == processCount.totalThread && finish < total)
                    {
                        finish = total;
                    }
                    for (var i = start; i < finish; i++)
                    {
                        processPool[i].NextTick();
                    }
                }
            }, processCount);
            thd.Start();
        }
    }

    public class ProcessThreadCount
    {
        public int currentThread { get; } = 0;
        public int totalThread { get; } = 0;
        public ProcessThreadCount(int currentThread, int totalThread)
        {
            this.currentThread = currentThread;
            this.totalThread = totalThread;

        }
    }
}



