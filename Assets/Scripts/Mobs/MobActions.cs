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
            GameManager.Instance.ControlLock = true;

            switch (action)
            {
                case ActionType.Attack:
                    GameManager.Instance.SelectingState = SelectingState.Enemy;
                    GameManager.Instance.SetCardPanel(true);
                    break;
                case ActionType.Skill:
                    switch (ParentMob.MobData.AttackType)
                    {
                        case AttackType.Melee:
                            GameManager.Instance.SelectingState = SelectingState.Enemy;
                            GameManager.Instance.SetCardPanel(true);
                            break;
                        case AttackType.Ranged:
                            GameManager.Instance.SelectingState = SelectingState.Enemy;
                            GameManager.Instance.SetCardPanel(true);
                            break;
                        case AttackType.Heal:
                            GameManager.Instance.SelectingState = SelectingState.Player;
                            break;
                        case AttackType.UnStun:
                            GameManager.Instance.SelectingState = SelectingState.Player;
                            break;
                        case AttackType.CastShield:
                            GameManager.Instance.SelectingState = SelectingState.Player;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
            
            ParentMob.UI.ShowTargetCursor();
            GameManager.Instance.PickingMob = ParentMob;
        }
        
        public void ActionPrepared()
        {
            ParentMob.State = MobState.Ready;
            ParentMob.MobMovement.MoveToRivalPosition();
            GameManager.Instance.ReadyToFight();
            UIManager.Instance.UISounds.ActionConfirm();

            GameManager.Instance.ExitChangeCardMode();
            CardPanel.Instance.DeleteCards();
            
            Debug.Log($"Action {ParentMob.CurrentAction.MobActionType} is prepared {ParentMob} is {ParentMob.State}");
        }

        public void PerformSkipTurn()
        {
            ParentMob.UI.ShowText("+" + ParentMob.StaminaRestoreAmount + " stamina!", Color.green);
            ParentMob.MobStamina += ParentMob.StaminaRestoreAmount;
            if (ParentMob.MobStamina > ParentMob.MobData.MaxStamina) ParentMob.MobStamina = ParentMob.MobData.MaxStamina;
            NextAction();
        }

        public void PerformStun()
        {
            ParentMob.UI.ShowText("Stunned!", Color.white);
        }

        public void PerformDefense()
        {
            ParentMob.MobStamina -= ParentMob.MobData.DefenseCost;
            ParentMob.MobStatusEffects.AddEffect(ParentMob, StatusEffectType.Defense, 1);
        }
        
        public void PerformAttack()
        {
            ParentMob.State = MobState.Attack;
            ParentMob.SoundController.StartMove();
            ParentMob.AnimationController.PlayRun_Animation();
            ParentMob.MobMovement.MoveToEnemy();
        }

        public void PerformSkill()
        {
            ParentMob.State = MobState.Attack;
            switch (ParentMob.MobData.AttackType)
            {
                case AttackType.Melee:
                    ParentMob.SoundController.StartMove();
                    ParentMob.AnimationController.PlayRun_Animation();
                    ParentMob.MobMovement.MoveToEnemy();
                    break;
                case AttackType.Ranged:
                    ParentMob.AnimationController.PlayActionAnimation();
                    break;
                case AttackType.Heal:
                case AttackType.UnStun:
                case AttackType.CastShield:
                    ParentMob.AnimationController.PlayActionAnimation();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void MakeDamage()
        {
            switch (ParentMob.CurrentAction.MobActionType)
            {
                case ActionType.Attack:
                    StartCoroutine(DamageCoroutine(ParentMob.MobData.AttackDamage, ParentMob.MobData.AttackCost));
                    break;

                case ActionType.Skill:
                    StartCoroutine(DamageCoroutine(ParentMob.MobData.SkillDamage, ParentMob.MobData.SkillCost));
                    break;
            } 
        }

        private void MakeSkill()
        {
            switch (ParentMob.MobData.AttackType)
            {
                case AttackType.Melee:
                    MakeDamage();
                    break;
                case AttackType.Ranged:
                    MakeDamage();
                    break;
                case AttackType.Heal:
                case AttackType.UnStun:
                case AttackType.CastShield:
                    StartCoroutine(SkillCoroutine(ParentMob.MobData.SkillDamage, ParentMob.MobData.SkillCost));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private IEnumerator DamageCoroutine(float damage, float cost)
        {
            ParentMob.CurrentAction.TargetInstance.MobCombatSystem.GetDamage(damage, ParentMob.CurrentCombo);
            ParentMob.MobStamina -= cost;

            yield return new WaitForSeconds(.5f);
            ParentMob.MobMovement.GoToOriginPosition(true);

            yield return new WaitForSeconds(1);
            NextAction();
        }

        private IEnumerator SkillCoroutine(float damage, float cost)
        {
            switch (ParentMob.MobData.AttackType)
            {
                case AttackType.Heal:
                    ParentMob.CurrentAction.TargetInstance.MobCombatSystem.Heal(damage);
                    break;
                case AttackType.UnStun:
                    ParentMob.CurrentAction.TargetInstance.MobCombatSystem.UnStun();
                    break;
                case AttackType.CastShield:
                    ParentMob.CurrentAction.TargetInstance.MobStatusEffects.AddEffect(ParentMob.CurrentAction.TargetInstance, StatusEffectType.Defense, 1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            ParentMob.MobStamina -= cost;

            yield return new WaitForSeconds(.5f);
            ParentMob.MobMovement.GoToOriginPosition(true);

            yield return new WaitForSeconds(1);
            NextAction();
        }
        
        public void NextAction()
        {
            QueueManager.Instance.NextAction();
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
                    if (ParentMob.MobStamina >= ParentMob.MobData.SkillCost) return true;
                    break;
            }

            return false;
        }
    }
}
