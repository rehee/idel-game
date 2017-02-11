using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Service.GameService;
using SpriteModule.Modules.MonsterModules;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace learnCoreMoltiThread.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        private IGameService gameService;
        public HomeController(IGameService gameService)
        {
            this.gameService = gameService;
        }
        public IActionResult Index()
        {
            //var result = WorkerServer.WorkerProcesserService.Que[1].GetWorker();
            //var monster = WorkerServer.WorkerProcesserService.Que[1].GetTarget();
            //return Content($"level: {result.Level.ToString()},attack: {result.Attack.ToString()},hp: {result.Hp.ToString()},exp: {result.WorkerExp.ToString()},next: {(result.NextExperience - result.WorkerExp).ToString()} monster exp {monster.Experience.ToString()}");
            return Redirect("/index.html");
        }

        public JsonResult GetResult(int? id)
        {
            try
            {
                var player = gameService.Que[(int)id].GetWorker();
                var monster = (IMonsterBase)gameService.Que[(int)id].GetTarget();
                var playerHp = player.Attribute.Hp.ToString();
                var result = new
                {
                    player = new
                    {
                        level = player.Attribute.Level,
                        hp = playerHp,
                        exp = player.WorkerExp.ToString(),
                        next = player.NextExperience.ToString(),
                        inAttack = player.Status.Attacking,
                        damage = player.Status.Damage.ToString(),
                        attackName = player.Status.AttackName
                    },
                    monster = new
                    {
                        name = monster.Name,
                        attack = monster.Attribute.Attack.ToString(),
                        exp = ((MonsterModel)monster).Experience.ToString(),
                        hp = monster.Attribute.Hp.ToString(),
                        damage = monster.Status.Damage.ToString(),
                        inAttack = monster.Status.Attacking
                    },
                    date = gameService.Que[1].ProcessDateTime()
                };
                return Json(result);
            }
            catch { return null; }
            
        }
    }
}
