using Core.Logics.Manage;
using System;
using System.Collections.Generic;
using System.Extend;
using System.Linq;
using System.Service.StageService;
using System.Threading.Tasks;

namespace Logic.Stage
{
    public class StageManager : IStageManager
    {

        private IEnveroment env;
        private IStageService stageService;
        public StageManager(IEnveroment env, IStageService stageService)
        {
            this.env = env;
            this.stageService = stageService;
        }

        public void AddStage(string stageName,out int stageId)
        {
            try
            {
                var newStage = env.NewStage();
                newStage.StageName = stageName;
                this.stageService.AddStage(newStage);
                stageId = newStage.StageId;
            }
            catch { stageId = -1; }
        }
        public void ChangeStageName(int stageId, string stageName)
        {
            try
            {
                var stage = stageService.GetStageById(stageId);
                stage.StageName = stageName;
            }
            catch { }
        }
        public IList<IStageBase> GetAllStage()
        {
            return stageService.Stages;
        }
        public List<IStageBase> GetStage(string stageName)
        {
            try
            {
                return this.stageService.Stages.Where(b => b.StageName.IndexOf(stageName) >= 0).ToList();
            }
            catch { return new List<IStageBase>(); }
            
        }
        public IStageBase GetStage(int stageId)
        {
            try
            {
                return this.stageService.Stages.Where(b => b.StageId == stageId).FirstOrDefault();
            }
            catch { return null; }
        }
    }
}
