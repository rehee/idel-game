using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Service.StageService;
using Core.Logics.Manage;
using System.Net.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace learnCoreMoltiThread.Controllers
{
    [Route("api/[controller]")]
    public class MapsController : Controller
    {
        private IStageService stageService;
        private IStageManager stagemanager;
        public MapsController(IStageService stageService, IStageManager stagemanager)
        {
            this.stageService = stageService;
            this.stagemanager = stagemanager;
        }
        // GET: api/values
        [HttpGet]
        public JsonResult Get()
        {
            var maps = stagemanager.GetAllStage();
            return Json(maps.Select(b=> { return new { id=b.StageId, name = b.StageName, monsterList = b.MonsterList.Select(c=>c.Name).ToList() }; }).ToList());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            var maps = stagemanager.GetStage(id);
            return Json( 
                new
                {
                    id = maps.StageId,
                    name = maps.StageName,
                    monsterList = maps.MonsterList.Select(c => c.Name).ToList()
                });
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]mapPost data)
        {
            //var stageName = request.Content.ReadAsStringAsync().Result;
            int stageId;
            this.stagemanager.AddStage(data.stageName, out stageId);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]mapPost data)
        {
            this.stagemanager.ChangeStageName(id, data.stageName);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
    public class mapPost
    {
        public string stageName { get; set; }
    }
}
