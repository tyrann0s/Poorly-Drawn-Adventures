using System;
using System.Collections;
using Cards;
using Managers;
using Mobs.Status_Effects;
using UnityEngine;

namespace Mobs
{
    public class MobActions : MobComponent
    {
        // События
        public static Action<Mob, Mob> OnAttack;
        public static Action<Mob, Mob, float> OnDeflect;
        public static Action<Mob, Mob, float, bool> OnDamage;
        
        public void SkipTurn()
        {
            ParentMob.CurrentAction.MobActionType = ActionType.SkipTurn;
            ParentMob.CurrentAction.MobInstance = ParentMob;
            ParentMob.Deactivate();

            ParentMob.UI.Buttons.BlockButtons(MobButtons.Buttons.SkipTurn);
            ActionPrepared();
        }

        public void Defense()
        {
            ParentMob.CurrentAction.MobActionType = ActionType.Defense;
            ParentMob.CurrentAction.MobInstance = ParentMob;
            ParentMob.Deactivate();
            
            ParentMob.UI.Buttons.BlockButtons(MobButtons.Buttons.Defense);
            ActionPrepared();
        }

        public void Attack()
        {
            ParentMob.UI.Buttons.BlockButtons(MobButtons.Buttons.Attack);
            CreateAction(ActionType.Attack);
        }

        public void Skill()
        {
            if (ParentMob.MobData.ActiveSkill is ResurrectSkill)
            {
                bool isSomeoneDead = false;
                foreach (var playerMob in ServiceLocator.Get<MobManager>().PlayerMobs)
                {
                    if (playerMob.State == MobState.Dead)
                    {
                        isSomeoneDead = true;
                        break;
                    }
                }
                            
                if (!isSomeoneDead)
                {
                    ParentMob.UI.ShowText("No one is dead!", Color.red);
                    ParentMob.Deactivate();
                } else CreateAction(ActionType.ActiveSkill);
            } else CreateAction(ActionType.ActiveSkill);

            ParentMob.UI.Buttons.BlockButtons(MobButtons.Buttons.Skill);
        }

        private void CreateAction(ActionType action)
        {
            ParentMob.CurrentAction.MobActionType = action;
            ParentMob.CurrentAction.MobInstance = ParentMob;
            ServiceLocator.Get<GameManager>().ControlLock = true;

            switch (action)
            {
                case ActionType.Attack:
                    ServiceLocator.Get<TargetManager>().SetContext(new TargetSelectionContext(SelectingState.Enemy, ParentMob, null, mob => mob.IsHostile && mob.State == MobState.Idle));
                    ServiceLocator.Get<CardPanel>().EnableInteraction();
                    break;
                case ActionType.ActiveSkill:
                    if (ParentMob.MobData.ActiveSkill.IsAttack)
                    {
                        ServiceLocator.Get<TargetManager>().SetContext(new TargetSelectionContext(
                            SelectingState.Enemy,
                            ParentMob, 
                            null, 
                            mob => mob.IsHostile && mob.State == MobState.Idle,
                            ParentMob.MobData.MaxTargets));
                        ServiceLocator.Get<CardPanel>().EnableInteraction();
                    }
                    else
                    {
                        ServiceLocator.Get<TargetManager>().SetContext(new TargetSelectionContext(
                            SelectingState.Player,
                            ParentMob,
                            null,
                            mob => !mob.IsHostile && mob.State == MobState.Idle,
                            ParentMob.MobData.MaxTargets));
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
            
            ParentMob.UI.ShowTargetCursor();
            ServiceLocator.Get<GameManager>().PickingMob = ParentMob;
        }
        
        public void ActionPrepared()
        {
            ParentMob.State = MobState.Ready;
            ParentMob.MobMovement.MoveToReadyPosition();
            ServiceLocator.Get<GameManager>().ReadyToFight();
            ServiceLocator.Get<UIManager>().UISounds.ActionConfirm();

            ServiceLocator.Get<CardPanel>().DisableInteraction();
            ServiceLocator.Get<CardPanel>().DeleteCards();
        }

        public void PerformSkipTurn()
        {
            ParentMob.UI.ShowText("Stamina Restored!", Color.green);
            ParentMob.RestoreStamina();
            ServiceLocator.Get<QueueManager>().NextAction();
        }

        public void PerformDefense()
        {
            StartCoroutine(DefenseCoroutine());
        }
        
        public void PrepareAction(ActionType actionType)
        {
            if (ParentMob.State == MobState.Dead) return;
            ParentMob.State = MobState.Attack;
            
            switch (actionType)
            {
                case ActionType.Attack:
                    ParentMob.SoundController.StartMove();
                    ParentMob.AnimationController.PlayRun_Animation();
                    ParentMob.MobMovement.MoveToEnemy(true);
                    break;
                
                case ActionType.ActiveSkill:
                    if (!ParentMob.MobData.ActiveSkill.IsRanged)
                    {
                        ParentMob.SoundController.StartMove();
                        ParentMob.AnimationController.PlayRun_Animation();
                        ParentMob.MobMovement.MoveToEnemy(true);
                    } else ParentMob.AnimationController.PlayActionAnimation(true);
                    break;
                
                case ActionType.PassiveSkill:
                    if (!ParentMob.MobData.PassiveSkill.IsRanged)
                    {
                        ParentMob.SoundController.StartMove();
                        ParentMob.AnimationController.PlayRun_Animation();
                        ParentMob.MobMovement.MoveToEnemy(false);
                    } else ParentMob.AnimationController.PlayActionAnimation(false);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(actionType), actionType, null);
            }
        }

        public void MakeDamage()
        {
            StartCoroutine(DamageCoroutine(ParentMob.MobData.AttackDamage, ParentMob.MobData.AttackCost));
        }

        public void MakeActiveSkill()
        {
            StartCoroutine(ActiveSkillCoroutine());
        }

        public void MakePassiveSkill()
        {
            StartCoroutine(PassiveSkillCoroutine());
        }

        private IEnumerator DamageCoroutine(float damage, float cost)
        {
            ParentMob.CurrentAction.TargetInstance.MobCombatSystem.GetDamage(damage, ParentMob.CurrentCombo);
            if (ParentMob.CurrentAction.TargetInstance.MobStatusEffects.CheckShield()) 
                OnDeflect?.Invoke(ParentMob.CurrentAction.TargetInstance, ParentMob, damage);
            else
            {
                OnDamage?.Invoke(ParentMob.CurrentAction.TargetInstance, ParentMob, damage, true);
                OnAttack?.Invoke(ParentMob, ParentMob.CurrentAction.TargetInstance);
            }
            ParentMob.SpendStamina(cost);

            yield return new WaitForSeconds(.5f);
            ParentMob.MobMovement.GoToOriginPosition(true);

            yield return new WaitForSeconds(1);
            ServiceLocator.Get<QueueManager>().NextAction();
        }

        private IEnumerator ActiveSkillCoroutine()
        {
            ParentMob.MobData.ActiveSkill.Use(ParentMob.CurrentAction.TargetInstance);
            if (ParentMob.IsHostile != ParentMob.CurrentAction.TargetInstance.IsHostile)
            {
                if (ParentMob.CurrentAction.TargetInstance.MobStatusEffects.CheckShield())
                    OnDeflect?.Invoke(ParentMob.CurrentAction.TargetInstance, ParentMob, ParentMob.MobData.ActiveSkill.Amount);
                else if (ParentMob.MobData.ActiveSkill.IsRanged)
                {
                    OnDamage?.Invoke(ParentMob.CurrentAction.TargetInstance, ParentMob, ParentMob.MobData.ActiveSkill.Amount, false);
                    OnAttack?.Invoke(ParentMob, ParentMob.CurrentAction.TargetInstance);
                }
                else
                {
                    OnDamage?.Invoke(ParentMob.CurrentAction.TargetInstance, ParentMob, ParentMob.MobData.ActiveSkill.Amount, true);
                    OnAttack?.Invoke(ParentMob, ParentMob.CurrentAction.TargetInstance);
                }
            }
            ParentMob.SpendStamina(ParentMob.MobData.ActiveSkill.Cost/ParentMob.MobData.MaxTargets);
            
            yield return new WaitForSeconds(.5f);
            ParentMob.MobMovement.GoToOriginPosition(true);

            yield return new WaitForSeconds(1);
            ServiceLocator.Get<QueueManager>().NextAction();
        }

        private IEnumerator PassiveSkillCoroutine()
        {
            ParentMob.MobData.PassiveSkill.Use(ParentMob.PassiveAction.TargetInstance);

            yield return new WaitForSeconds(.5f);
            ServiceLocator.Get<QueueManager>().NextAction();
        }

        private IEnumerator DefenseCoroutine()
        {
            ParentMob.SpendStamina(ParentMob.MobData.DefenseCost);
            ParentMob.MobStatusEffects.AddEffect(StatusEffectType.Defense, 1);
            
            yield return new WaitForSeconds(1);
            ServiceLocator.Get<QueueManager>().NextAction();
        }
        
        public bool CheckStamina()
        {
            switch (ParentMob.CurrentAction.MobActionType)
            {
                case ActionType.SkipTurn:
                    return true;

                case ActionType.Defense:
                    if (ParentMob.CheckStamina(ParentMob.MobData.DefenseCost)) return true;
                    break;

                case ActionType.Attack:
                    if (ParentMob.CheckStamina(ParentMob.MobData.AttackCost)) return true;
                    break;

                case ActionType.ActiveSkill:
                    if (ParentMob.CheckStamina(ParentMob.MobData.ActiveSkill.Cost)) return true;
                    break;
            }

            return false;
        }
    }
}
