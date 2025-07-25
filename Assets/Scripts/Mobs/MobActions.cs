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
            CreateAction(ActionType.Skill);
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
                case ActionType.Skill:
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
            ParentMob.State = MobState.Attack;
            
            switch (actionType)
            {
                case ActionType.Attack:
                    ParentMob.SoundController.StartMove();
                    ParentMob.AnimationController.PlayRun_Animation();
                    ParentMob.MobMovement.MoveToEnemy();
                    break;
                case ActionType.Skill:
                    if (!ParentMob.MobData.ActiveSkill.IsRanged)
                    {
                        ParentMob.SoundController.StartMove();
                        ParentMob.AnimationController.PlayRun_Animation();
                        ParentMob.MobMovement.MoveToEnemy();
                    } else ParentMob.AnimationController.PlayActionAnimation();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(actionType), actionType, null);
            }
        }

        private void MakeDamage()
        {
            StartCoroutine(DamageCoroutine(ParentMob.MobData.AttackDamage, ParentMob.MobData.AttackCost));
        }

        private void MakeSkill()
        {
            StartCoroutine(SkillCoroutine());
        }

        private IEnumerator DamageCoroutine(float damage, float cost)
        {
            ParentMob.CurrentAction.TargetInstance.MobCombatSystem.GetDamage(damage, ParentMob.CurrentCombo);
            ParentMob.MobStamina -= cost;

            yield return new WaitForSeconds(.5f);
            ParentMob.MobMovement.GoToOriginPosition(true);

            yield return new WaitForSeconds(1);
            ServiceLocator.Get<QueueManager>().NextAction();
        }

        private IEnumerator SkillCoroutine()
        {
            ParentMob.MobStamina -= ParentMob.MobData.ActiveSkill.Cost;

            ParentMob.MobData.ActiveSkill.Use(ParentMob.CurrentAction.TargetInstance);
            
            yield return new WaitForSeconds(.5f);
            ParentMob.MobMovement.GoToOriginPosition(true);

            yield return new WaitForSeconds(1);
            ServiceLocator.Get<QueueManager>().NextAction();
        }

        private IEnumerator DefenseCoroutine()
        {
            ParentMob.MobStamina -= ParentMob.MobData.DefenseCost;
            ParentMob.MobStatusEffects.AddEffect(ParentMob, StatusEffectType.Defense, 1);
            
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

                case ActionType.Skill:
                    if (ParentMob.MobStamina >= ParentMob.MobData.ActiveSkill.Cost) return true;
                    break;
            }

            return false;
        }
    }
}
