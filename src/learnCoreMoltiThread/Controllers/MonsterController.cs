using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Core.Logics.Manage;
using System.Numerics;
using System.Extend;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace learnCoreMoltiThread.Controllers
{
    [Route("api/[controller]")]
    public class MonsterController : Controller
    {
        // GET: api/values
        private IMonsterManager monsterManager;
        private IEnveroment env;
        public MonsterController(IMonsterManager monsterManager, IEnveroment env)
        {
            this.monsterManager = monsterManager;
            this.env=env;
    }

        [HttpGet]
        public JsonResult Get()
        {
            return Json(
                this.monsterManager.GetAllMonsterInPool().Select(b=>b.ConvertIMonsterBaseToApiModel()).ToList()
                );
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            try
            {
                return Json(
                this.monsterManager.GetMonsterInPoolById(id).ConvertIMonsterBaseToApiModel()
                );
            }
            catch
            {
                return Json(null);
            }
            
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]MonsterApiModel data)
        {
            var monster = env.NewMonster();
            data.ConvertMonsterApiModelBaseToIBase(monster);
            this.monsterManager.AddMonsterInPool(monster);
        }

        [HttpPost("ResetMonster")]
        public void ResetMonster()
        {
            this.monsterManager.ResetMonster();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]MonsterApiModel data)
        {
            try
            {
                var monster = env.NewMonster();
                data.ConvertMonsterApiModelBaseToIBase(monster);
                monsterManager.GetMonsterInPoolById(id).EditMonster(monster);
            }
            catch { }
            
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }


}

namespace System
{
    public static class apiExtend
    {
        public static MonsterApiModel ConvertIMonsterBaseToApiModel(this IMonsterBase monster)
        {
            try
            {
                var result = new MonsterApiModel()
                {
                    id = monster.Id,
                    name = monster.Name,
                    exp = monster.Experience.ToString(),
                    stageConfig = monster.StageConfig,
                    attack = monster.Attribute.Attack.ToString(),
                    hp = monster.Attribute.Hp.ToString(),
                    speed = monster.Status.AttackSpeed
                };
                return result;
            }
            catch { return null; }
        }

        public static void ConvertMonsterApiModelBaseToIBase(this MonsterApiModel apiModel, IMonsterBase monster)
        {
            try
            {
                monster.Id = apiModel.id;
                monster.Name = apiModel.name;
                monster.StageConfig = apiModel.stageConfig;
                monster.Status.AttackSpeed = apiModel.speed;

                BigInteger temp;

                BigInteger.TryParse(apiModel.exp, out temp);
                monster.Experience = temp;

                BigInteger.TryParse(apiModel.attack, out temp);
                monster.Attribute.Attack =  temp;

                BigInteger.TryParse(apiModel.hp, out temp);
                monster.Attribute.Hp = temp;

                return;
            }
            catch { return; }
        }
    }

    public class MonsterApiModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string hp { get; set; }
        public string attack { get; set; }
        public int speed { get; set; }
        public string exp { get; set; }
        public Dictionary<int,int> stageConfig { get; set; }
    }
}
