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

            ActionPrepared();
        }

        public void Defense()
        {
            ParentMob.CurrentAction.MobActionType = ActionType.Defense;
            ParentMob.CurrentAction.MobInstance = ParentMob;
            ParentMob.Deactivate();

            ActionPrepared();
        }

        public void Attack()
        {
            CreateAction(ActionType.Attack);
        }

        public void Skill()
        {
            CreateAction(ActionType.ActiveSkill);
        }

        private void CreateAction(ActionType action)
        {
            ParentMob.CurrentAction.MobActionType = action;
            ParentMob.CurrentAction.MobInstance = ParentMob;
            ServiceLocator.Get<GameManager>().ControlLock = true;

            switch (action)
            {
                case ActionType.Attack:
                    ServiceLocator.Get<GameManager>().SelectingState = SelectingState.Enemy;
                    ServiceLocator.Get<CardPanel>().EnableInteraction();
                    break;
                case ActionType.ActiveSkill:
                    if (ParentMob.MobData.ActiveSkill.IsAttack)
                    {
                        ServiceLocator.Get<GameManager>().SelectingState = SelectingState.Enemy;
                        ServiceLocator.Get<CardPanel>().EnableInteraction();
                    }
                    else
                    {
                        ServiceLocator.Get<GameManager>().SelectingState = SelectingState.Player;
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
            ParentMob.MobMovement.MoveToRivalPosition();
            ServiceLocator.Get<GameManager>().ReadyToFight();
            ServiceLocator.Get<UIManager>().UISounds.ActionConfirm();

            ServiceLocator.Get<CardPanel>().DisableInteraction();
            ServiceLocator.Get<CardPanel>().DeleteCards();
            
            Debug.Log($"Action {ParentMob.CurrentAction.MobActionType} is prepared {ParentMob} is {ParentMob.State}");
        }

        public void PerformSkipTurn()
        {
            ParentMob.UI.ShowText("+" + ParentMob.StaminaRestoreAmount + " stamina!", Color.green);
            ParentMob.MobStamina += ParentMob.StaminaRestoreAmount;
            if (ParentMob.MobStamina > ParentMob.MobData.MaxStamina) ParentMob.MobStamina = ParentMob.MobData.MaxStamina;
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

        private void MakeDamage()
        {
            StartCoroutine(DamageCoroutine(ParentMob.MobData.AttackDamage, ParentMob.MobData.AttackCost));
        }

        private void MakeActiveSkill()
        {
            StartCoroutine(ActiveSkillCoroutine());
        }

        private void MakePassiveSkill()
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
            ParentMob.MobStamina -= cost;

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
            ParentMob.MobStamina -= ParentMob.MobData.ActiveSkill.Cost;
            
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
            ParentMob.MobStamina -= ParentMob.MobData.DefenseCost;
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
                    if (ParentMob.MobStamina >= ParentMob.MobData.DefenseCost) return true;
                    break;

                case ActionType.Attack:
                    if (ParentMob.MobStamina >= ParentMob.MobData.AttackCost) return true;
                    break;

                case ActionType.ActiveSkill:
                    if (ParentMob.MobStamina >= ParentMob.MobData.ActiveSkill.Cost) return true;
                    break;
            }

            return false;
        }
    }
}
