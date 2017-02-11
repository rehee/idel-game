using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace System
{
    public class FullActionLog
    {
        public DateTime Time { get; } = DateTime.Now;
        public ActionItem Item { get; }
        public FullActionLog(ActionItem item = null)
        {
            this.Item = item ?? new ActionItem();
        }
    }
    public class ActionItem
    {
        public string ActionFrom { get; }
        public string ActionTarget { get; }
        public ActionTypeOption ActionType { get; }
        public string ActionName { get; }
        public string Result { get; }
        public ActionItem(string actionFrom = "", string actionTarget = "", ActionTypeOption type = ActionTypeOption.NormalAttack, string actionName = "", string result = "")
        {
            this.ActionFrom = actionFrom;
            this.ActionTarget = actionTarget;
            this.ActionType = type;
            this.ActionName = actionName;
            this.Result = result;
        }
    }

    public enum ActionTypeOption
    {
        NormalAttack = 0,
        CastSkill = 1,
        Dead = 2,
        GetExp = 3,
        LevelUp = 4
    }
    public class DamageLog
    {
        public ISpritBase Character { get; set; }
        public BigInteger TotalDamage { get; set; }
    }
}
