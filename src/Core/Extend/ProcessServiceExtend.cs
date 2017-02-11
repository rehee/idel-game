using System;
using System.Collections.Generic;
using System.Linq;
using System.Service.ProcessService;
using System.Threading.Tasks;

namespace System.Extend
{
    public static class ProcessServiceExtend
    {
        public static ProcessServiceJsonMode ToJsonMode(this IProcessService process)
        {
            var jsonMode = new ProcessServiceJsonMode();
            if (process == null)
                goto finish;
            jsonMode.player = process.GetWorker().ToJsonMode();
            jsonMode.currentMapId = process.CurrentStage;
            jsonMode.target = ((IMonsterBase)process.GetTarget()).ToJsonMode();
            jsonMode.combatLog = process.GetWorker().CombatLog;

            //var count = 0;
            //getCombatLog:
            //try
            //{
            //    jsonMode.combatLog = process.GetWorker().CombatLog.GetIEnumerableByIndex().ToList().OrderByDescending(b => b.Time).FirstOrDefault();
            //}
            //catch
            //{
            //    count++;
            //    if(count<10)
            //        goto getCombatLog;
            //}

            finish:
            return jsonMode;
        }

    }
}
