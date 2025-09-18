using System;
using System.Globalization;
using System.Linq;
using Cards;
using Managers;
using Managers.Base;
using Mobs.Status_Effects;
using UnityEngine;

namespace Mobs
{
    public class MobCombatSystem : MobComponent
    {
        public void GetDamage(float damage, ElementCombo enemyCombo)
        {
            if (ParentMob.State == MobState.Dead) return;

            if (enemyCombo && enemyCombo.aoeAttack)
            {
                var mobsToDamage = ServiceLocator.Get<MobManager>().EnemyMobs.ToList();
                foreach (var enemyMob in mobsToDamage)
                {
                    enemyMob.MobCombatSystem.ApplyDamage(damage, enemyCombo);
                }
            } else ApplyDamage(damage, enemyCombo);
        }
    
        public bool HandleShieldAttack(ElementCombo enemyCombo)
        {
            if (enemyCombo)
            {
                if (ParentMob.MobStatusEffects.CheckShield())
                {
                    if (enemyCombo.ignoreDefense)
                    {
                        ParentMob.MobStatusEffects.StatusEffects.RemoveAll(x => x.EffectType == StatusEffectType.Defense);
                        ParentMob.UI.RemoveStatusEffect(new SEDefense());
                        ParentMob.UI.HideShield();
                        ParentMob.State = MobState.Idle;
                        return false;
                    }

                    ParentMob.SoundController.PlayShieldDamageSound();
                    ParentMob.UI.ShowText("Blocked!", Color.white);
                    return true;
                }
                return false;
            }

            if (ParentMob.MobStatusEffects.CheckShield())
            {
                ParentMob.SoundController.PlayShieldDamageSound();
                ParentMob.UI.ShowText("Blocked!", Color.white);
                return true;
            }

            return false;
        }

        public bool HandleEvasion()
        {
            if (ParentMob.MobData.PassiveSkill is EvasionSkill)
            {
                var evasionSkill = ParentMob.MobData.PassiveSkill as EvasionSkill;
                float rand = UnityEngine.Random.value;
                
                if (rand > evasionSkill.Chance) return false;
                ParentMob.UI.ShowText("Evasion!", Color.white);
                return true;
            } return false;
        }
    
        private void ApplyDamage(float damage, ElementCombo enemyCombo)
        {
            if (ParentMob.IsHostile && enemyCombo) ProgressManager.Instance.UnlockRecord(enemyCombo);
            
            if (HandleShieldAttack(enemyCombo)) return;
            if (HandleEvasion()) return;

            float resultDamage;
            
            // Логика атаки с комбо
            if (enemyCombo)
            {
                Color color;
                switch (enemyCombo.elementType)
                {
                    case ElementType.Physical:
                        color = Color.white;
                        break;
                    case ElementType.Etherial:
                        color = Color.magenta;
                        break;
                    case ElementType.Fire:
                        color = Color.red;
                        break;
                    case ElementType.Water:
                        color = Color.blue;
                        break;
                    case ElementType.Air:
                        color = Color.cyan;
                        break;
                    case ElementType.Earth:
                        color = Color.green;
                        break;
                    default:
                        Debug.Log(enemyCombo.elementType);
                        throw new ArgumentOutOfRangeException();
                }

                if (CheckImmune(enemyCombo.elementType))
                {
                    if (ParentMob.IsHostile) ProgressManager.Instance.UnlockRecord(ParentMob.MobData, false, true);
                    ParentMob.UI.ShowText("Immune!", Color.white);
                    return;
                }

                if (ParentMob.MobData.VulnerableTo == enemyCombo.elementType)
                {
                    if (ParentMob.IsHostile) ProgressManager.Instance.UnlockRecord(ParentMob.MobData, true, false);
                    resultDamage = damage * (enemyCombo.damageMultiplier * 2f);
                    ParentMob.SoundController.PlayGetDamageSound(true);
                    ParentMob.UI.ShowText("CRITICAL! " + resultDamage, color);
                }
                else
                {
                    resultDamage = damage * enemyCombo.damageMultiplier;
                    ParentMob.SoundController.PlayGetDamageSound(false);
                    ParentMob.UI.ShowText(resultDamage.ToString(CultureInfo.CurrentCulture), color);
                }

                if (enemyCombo.stun)
                {
                    ParentMob.MobStatusEffects.AddEffect(StatusEffectType.Stun, enemyCombo.stunTime);
                    ParentMob.UI.ShowText($"Stunned for {enemyCombo.stunTime} turns!", Color.white);
                }
            }
            // Логика атаки без комбо
            else
            {
                if (CheckImmune(ElementType.Physical))
                {
                    if (ParentMob.IsHostile) ProgressManager.Instance.UnlockRecord(ParentMob.MobData, false, true);
                    ParentMob.UI.ShowText("Immune!", Color.white);
                    return;
                }

                if (ParentMob.MobData.VulnerableTo == ElementType.Physical)
                {
                    if (ParentMob.IsHostile) ProgressManager.Instance.UnlockRecord(ParentMob.MobData, true, false);
                    resultDamage = damage * 2f;
                    ParentMob.SoundController.PlayGetDamageSound(true);
                    ParentMob.UI.ShowText("CRITICAL! " + resultDamage, Color.white);
                }
                else
                {
                    resultDamage = damage;
                    ParentMob.SoundController.PlayGetDamageSound(false);
                    ParentMob.UI.ShowText(resultDamage.ToString(CultureInfo.CurrentCulture), Color.white);
                }
            }

            // Анимация получения урона
            ParentMob.AnimationController.PlayGetDamage_Animation();

            // Звук получения урона
            ParentMob.SoundController.GetDamage();

            // Визуальный эффект
            ParentMob.MobVFX.PlayGetDamageVFX();

            ParentMob.MobHP -= resultDamage;

            ParentMob.UI.UpdateHP(ParentMob.MobHP);

            if (ParentMob.MobHP <= 0) Die();
        }

        public void Heal(float healAmount)
        {
            if (ParentMob.State == MobState.Dead) return;
            ParentMob.MobHP += healAmount;
            if (ParentMob.MobHP > ParentMob.MobData.MaxHP) ParentMob.MobHP = ParentMob.MobData.MaxHP;
            ParentMob.UI.UpdateHP(ParentMob.MobHP);
            ParentMob.UI.ShowText("Healed!", Color.green);
        }
    
        private void Die()
        {
            ParentMob.State = MobState.Dead;
            ParentMob.SoundController.Death();
            if (ParentMob.UI)
            {
                ParentMob.UI.MobDeath();
            }

            ServiceLocator.Get<MobManager>().MobDied(ParentMob);
            ParentMob.AnimationController.PlayDie_Animation();
            ParentMob.MobVFX.PlayDeathVFX();
        }

        public void Revive()
        {
            ParentMob.State = MobState.Idle;
            ParentMob.MobHP = ParentMob.MobData.MaxHP;
            
            ParentMob.UI.MobResurrect();
            ParentMob.UI.UpdateHP(ParentMob.MobHP);
            
            ParentMob.AnimationController.PlayIdle_Animation();
            
            ParentMob.UI.ShowText("Resurrected!", Color.green);
        }

        private bool CheckImmune(ElementType elementType)
        {
            if (ParentMob.MobData.TotalImmune && elementType != ParentMob.MobData.VulnerableTo) return true;
            if (ParentMob.MobData.ImmuneTo == elementType) return true;
            
            return false;
        }
    }
}
