using Core.Skill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Service.SkillService
{
    public interface ISkillService
    {
        List<ISkillModel> SkillPool { get; }
        void CreateSkill(ISkillModel skill);
        ISkillModel GetSkillById(int id);
        ISkillModel GetSkillByName(string name);

    }
}
