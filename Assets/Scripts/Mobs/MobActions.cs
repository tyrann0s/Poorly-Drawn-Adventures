using System;
using System.Collections;
using Cards;
using Managers;
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
            Attack(ActionType.Attack);
        }

        public void Skill()
        {
            Attack(ActionType.Skill);
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

        private void Attack(ActionType action)
        {
            ParentMob.CurrentAction.MobActionType = action;
            ParentMob.CurrentAction.MobInstance = ParentMob;
            ParentMob.UI.ShowTargetCursor();
            GameManager.Instance.ControlLock = true;
            switch (ParentMob.MobData.AttackType)
            {
                case AttackType.Melee:
                    GameManager.Instance.SelectingState = SelectingState.Enemy;
                    break;
                case AttackType.Ranged:
                    GameManager.Instance.SelectingState = SelectingState.Enemy;
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
            GameManager.Instance.PickingMob = ParentMob;
            GameManager.Instance.ExitChangeCardMode();
            GameManager.Instance.SetCardPanel(true);
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
            ParentMob.State = MobState.Stun;
            ParentMob.UI.ShowText("Stunned!", Color.white);
            ParentMob.StunTime--;
        }

        public void PerformDefense()
        {
            ParentMob.MobStamina -= ParentMob.MobData.DefenseCost;
            ParentMob.State = MobState.Defense;
            ParentMob.UI.ShowText("Defense!", Color.blue);
            UIManager.Instance.UISounds.ShieldActivation();
            ParentMob.UI.ShowShield();
        }
        
        public void PerformAttack()
        {
            ParentMob.State = MobState.Attack;
            ParentMob.SoundController.StartMove();
            ParentMob.AnimationController.PlayRun_Animation();
            ParentMob.MobMovement.MoveToEnemy();
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

        private IEnumerator DamageCoroutine(float damage, float cost)
        {
            ParentMob.CurrentAction.TargetInstance.MobCombatSystem.GetDamage(damage, ParentMob.CurrentCombo);
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
                    else return false;

                case ActionType.Attack:
                    if (ParentMob.MobStamina >= ParentMob.MobData.AttackCost) return true;
                    else return false;

                case ActionType.Skill:
                    if (ParentMob.MobStamina >= ParentMob.MobData.SkillCost) return true;
                    else return false;
            }

            return false;
        }
    }
}
