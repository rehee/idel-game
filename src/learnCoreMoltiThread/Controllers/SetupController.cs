using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace learnCoreMoltiThread.Controllers
{
    public class SetupController : Controller
    {
        // GET: /<controller>/
        IEnveroment env;
        public SetupController(IEnveroment env)
        {
            this.env = env;
        }
        public IActionResult Index()
        {
            return Redirect("/index.html");
        }
    }
}
