using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Core.Logics.Control;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace learnCoreMoltiThread.Controllers
{
    [Route("api/console")]
    public class ConsoleController : Controller
    {
        IPlayerControl console;
        public ConsoleController(IPlayerControl console)
        {
            this.console = console;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost("new", Name = "{data}")]
        public JsonResult CreateNewChar([FromBody]postName data)
        {
            try
            {
                console.CreateChar(data.name);
                int currentId = -1;
                if (console.MyCharDetail != null)
                    currentId = console.MyCharDetail.Id;
                return Json(new { id = currentId });
            }
            catch { return null; }
            
        }
        [HttpGet("me/{id}")]
        public JsonResult GetMyChar(int? id)
        {
            try
            {
                console.SetChar((int)id);
                return Json(console.MyStatus());
            }
            catch { return null; }
        }
        [HttpPost("set", Name = "{id}")]
        public JsonResult SetChar([FromBody]int id)
        {
            console.SetChar(id);
            var myId = -1;
            if (console.MyCharDetail != null)
                myId = id;
            return Json(new { id = myId });

        }
        [HttpGet("where/{id}")]
        public JsonResult CurrentStage(int id)
        {
            console.SetChar(id);
            return Json(new { name= console.CurrentMap.StageName });
        }

        [HttpPost("goForward", Name ="{charIdF}")]
        public void ChangeStageForward([FromBody]int charIdF)
        {
            console.SetChar(charIdF);
            console.MoveForward();
        }
        [HttpPost("goback", Name = "{charIdB}")]
        public void ChangeStageBack([FromBody]int charIdB)
        {
            console.SetChar(charIdB);
            console.MoveBack();
        }




        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }


    }

    public class postName
    {
        public string name { get; set; }
    }
    
}
