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
            ParentMob.CurrentAction.MobAction = ActionType.SkipTurn;
            ParentMob.CurrentAction.MobInstance = ParentMob;
            ParentMob.Deactivate();

            ActionPrepared();
        }

        public void Defense()
        {
            ParentMob.CurrentAction.MobAction = ActionType.Defense;
            ParentMob.CurrentAction.MobInstance = ParentMob;
            ParentMob.Deactivate();

            ActionPrepared();
        }

        public void Attack1()
        {
            Attack(ActionType.Attack1);
        }

        public void Attack2()
        {
            Attack(ActionType.Attack2);
        }

        public void ActionPrepared()
        {
            ParentMob.State = MobState.Ready;
            ParentMob.MobMovement.MoveToRivalPosition();
            GameManager.Instance.ReadyToFight();
            UIManager.Instance.UISounds.ActionConfirm();

            GameManager.Instance.ExitChangeCardMode();
            CardPanel.Instance.DeleteCards();
            
            Debug.Log($"Action {ParentMob.CurrentAction.MobAction} is prepared {ParentMob} is {ParentMob.State}");
        }

        private void Attack(ActionType action)
        {
            ParentMob.CurrentAction.MobAction = action;
            ParentMob.CurrentAction.MobInstance = ParentMob;
            ParentMob.UI.ShowTargetCursor();
            GameManager.Instance.ControlLock = true;
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
            switch (ParentMob.CurrentAction.MobAction)
            {
                case ActionType.Attack1:
                    StartCoroutine(DamageCoroutine(ParentMob.MobData.Attack1Damage, ParentMob.MobData.Attack1Cost));
                    break;

                case ActionType.Attack2:
                    StartCoroutine(DamageCoroutine(ParentMob.MobData.Attack2Damage, ParentMob.MobData.Attack2Cost));
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
            switch (ParentMob.CurrentAction.MobAction)
            {
                case ActionType.SkipTurn:
                    return true;

                case ActionType.Defense:
                    if (ParentMob.MobStamina >= ParentMob.MobData.DefenseCost) return true;
                    else return false;

                case ActionType.Attack1:
                    if (ParentMob.MobStamina >= ParentMob.MobData.Attack1Cost) return true;
                    else return false;

                case ActionType.Attack2:
                    if (ParentMob.MobStamina >= ParentMob.MobData.Attack2Cost) return true;
                    else return false;
            }

            return false;
        }
    }
}
