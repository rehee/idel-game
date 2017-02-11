using Core.Service.SkillService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Skill;

namespace SkillServer
{
    public class SkillService : ISkillService
    {
        private static List<ISkillModel> skillPool { get; set; } = new List<ISkillModel>();
        public List<ISkillModel> SkillPool
        {
            get
            {
                return skillPool;   
            }
        }
        public void CreateSkill(ISkillModel skill)
        {
            try
            {
                var checkSkill = skillPool.OrderByDescending(b => b.SkillId).FirstOrDefault();
                if (checkSkill == null)
                {
                    skill.SkillId = 1;
                }
                else
                {
                    skill.SkillId = checkSkill.SkillId + 1;
                }
                skillPool.Add(skill);
            }
            catch { }
        }
        public ISkillModel GetSkillById(int id)
        {
            try
            {
                return skillPool.Where(b => b.SkillId == id).FirstOrDefault();
            }
            catch{ return null; }
        }
        public ISkillModel GetSkillByName(string name)
        {
            try
            {
                return skillPool.Where(b => b.SkillName == name).FirstOrDefault();
            }
            catch { return null; }
        }
    }
}
