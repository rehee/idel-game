using Core.Skill;
using Core.Spirit.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace System.Extend
{
    public static class SpritBaseExtend
    {
        public static int DamageRecordLimit = 30;
        public static int ActiionLogLimit = 30;
        public static int FaildCount = 30;

        public static bool PlayerAttackCastFinish(this ISpritBase sprit)
        {
            if (sprit.Target == null || sprit.Target.Attribute.Hp <= 0)
            {
                goto PlayerAttackCastFinish;
            }
            sprit.Status.AttackCount++;
            if (sprit.Status.AttackCount >= sprit.Status.AttackSpeed)
            {
                sprit.Status.AttackCount = 0;
                return true;
            }
            PlayerAttackCastFinish:
            sprit.NoAttack();
            return false;
        }
        public static void DoAttack(this ISpritBase attacker, ISpritBase target, string attackName = "普通攻击", ISkillinSpirit skill = null)
        {

            if (!CheckAttack(attacker, target))
                return;
            var actionType = ActionTypeOption.NormalAttack;
            AttackTypeOption attackType = AttackTypeOption.NormalAttack;
            BigInteger? extureDamage = null;
            try
            {
                if (skill != null)
                {
                    skill.CurrentCD = skill.skill.SkillCD;
                    attackName = skill.skill.SkillName;
                    extureDamage = skill.skill.Damage;
                    attackType = AttackTypeOption.SkillAttack;
                    actionType = ActionTypeOption.CastSkill;
                }
            }
            catch { }
            BigInteger totalDamage;
            CalculateDamage(attacker, target, attackName, extureDamage, attackType, out totalDamage);
            attacker.AddDamageRecord(target, totalDamage);
            attacker.AddActionLog(attacker.Name, target.Name, actionType, attackName, totalDamage.ToString());
            target.AddActionLog(attacker.Name, target.Name, actionType, attackName, totalDamage.ToString());
        }
        public static void CalculateDamage(this ISpritBase attacker, ISpritBase target, string attackName, BigInteger? extureDamage, AttackTypeOption attackType, out BigInteger totalDamage)
        {
            target.Attribute.Hp = target.Attribute.Hp - attacker.Attribute.Attack;
            attacker.Status.Damage = attacker.Attribute.Attack;
            if (extureDamage != null)
            {
                target.Attribute.Hp = target.Attribute.Hp - (BigInteger)extureDamage;
                attacker.Status.Damage = attacker.Status.Damage + (BigInteger)extureDamage;
            }
            attacker.Status.StatusType = SpriteStatusType.combat;
            target.Status.StatusType = SpriteStatusType.combat;
            attacker.Status.AttackName = attackName;
            attacker.Status.Attacking = true;
            attacker.Status.AttackType = attackType;
            totalDamage = attacker.Status.Damage;
        }
        public static bool CheckAttack(this ISpritBase attacker, ISpritBase target)
        {
            bool dead = false;
            if (target == null)
                goto NoneAttack;
            if (target.Attribute.Hp <= 0)
                goto NoneAttack;
            if (attacker.Attribute.Hp <= 0)
            {
                goto NoneAttack;
            }
            return true;
            NoneAttack:
            attacker.NoAttack();
            return false;
        }
        public static void NoAttack(this ISpritBase attacker, bool attackerDead = false)
        {
            if (attacker == null)
            {
                return;
            }
            try
            {
                attacker.Status.Attacking = false;
                attacker.Status.AttackName = "";
                attacker.Status.Damage = 0;
            }
            catch { }

            return;
        }

        public static void AddDamageRecord(this ISpritBase attacker, ISpritBase target, BigInteger totalDamage)
        {
            int count = 0;
            start:
            if (attacker == null)
            {
                return;
            }
            if (target.DamageRecord == null)
            {
                target.DamageRecord = new Dictionary<int, DamageLog>();
            }
            if (target.DamageRecord.ContainsKey(attacker.Id))
            {
                target.DamageRecord[attacker.Id].TotalDamage = target.DamageRecord[attacker.Id].TotalDamage + totalDamage;
            }
            else
            {
                try
                {
                    target.DamageRecord.Add(attacker.Id, new DamageLog() { Character = attacker, TotalDamage = totalDamage });
                }
                catch
                {
                    count++;
                    if (count > 10)
                    {
                        return;
                    }
                    else
                    {
                        goto start;
                    }
                }
            }
            count = 0;
            removeDamageRecord:
            try
            {
                if (target.DamageRecord.Count > DamageRecordLimit)
                {
                    var list = target.DamageRecord.OrderBy(b => b.Value.TotalDamage)
                        .Take(target.DamageRecord.Count - DamageRecordLimit)
                        .Select(b => b.Key).ToList();
                    foreach (var item in list)
                    {
                        target.DamageRecord.Remove(item);
                    }
                }
            }
            catch
            {
                count++;
                if (count > FaildCount)
                {
                    return;
                }
                else
                {
                    goto removeDamageRecord;
                }

            }

        }

        public static void ClearDamageRecord(this ISpritBase spirit)
        {
            //spirit.DamageRecord = new Dictionary<int, DamageLog>();
            spirit.DamageRecord.Clear();
        }

        public static void AddActionLog(this ISpritBase spirit, string actionFrom = "", string actionTarget = "", ActionTypeOption actionType = ActionTypeOption.NormalAttack, string actionName = "", string actionResult = "")
        {
            var actionItem = new ActionItem(actionFrom, actionTarget, actionType, actionName, actionResult);
            var actionLog = new FullActionLog(actionItem);
            spirit.ActionLog.AddLimit(actionLog, ActiionLogLimit);
        }


    }
}
