using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace System.Extend
{
    public static class IPlayerExtend
    {
        public static void AddExperience(this IPlayerBase player, BigInteger experience)
        {
            player.WorkerExp = player.WorkerExp + experience;
            while (player.WorkerExp >= player.NextExperience)
            {
                player.levelUp();
            }
        }
        public static void levelUp(this IPlayerBase player)
        {
            var worker = player;
            worker.WorkerExp = worker.NextExperience - worker.WorkerExp;
            if (worker.WorkerExp < 0)
                worker.WorkerExp = 0;
            worker.Attribute.Level++;
            worker.Attribute.Hp = worker.Attribute.Level * 100;
            worker.Attribute.Attack = worker.Attribute.Attack + worker.Attribute.Level;
            worker.NextExperience = GetNextExperience(worker.Attribute.Level);
        }
        public static BigInteger GetNextExperience(int level)
        {
            return 100 + level * 1000;
        }
        public static PlayerBaseJsonMode ToJsonMode(this IPlayerBase player)
        {
            var jsonMode = new PlayerBaseJsonMode();
            if (player == null)
                goto finish;
            try
            {
                jsonMode.id = player.Id;
                jsonMode.name = player.Name;
                jsonMode.level = player.Attribute.Level;
                jsonMode.hp = player.Attribute.Hp.ToString();
                jsonMode.attack = player.Attribute.Attack.ToString();
                jsonMode.attackSpeed = player.Status.AttackSpeed;
                jsonMode.currentExp = player.WorkerExp.ToString();
                jsonMode.nextExp = player.NextExperience.ToString();
                jsonMode.gapExp = (player.NextExperience - player.WorkerExp).ToString();
                jsonMode.petcentToNextLevel = (decimal)player.WorkerExp / (decimal)player.NextExperience;
                jsonMode.maxHp = player.Attribute.MaxHp.ToString();
                foreach (var skill in player.SkillPool)
                {
                    var skilllog = new SkillLog();
                    if (skill.Value != null && skill.Value.skill != null)
                    {
                        skilllog.cd = skill.Value.CurrentCD;
                        skilllog.maxCd = skill.Value.skill.SkillCD;
                        skilllog.skillName = skill.Value.skill.SkillName;
                    }
                    jsonMode.skills.Add(skill.Key, skilllog);
                }

            }
            catch { }
            finish:
            return jsonMode;
        }
    }
}
