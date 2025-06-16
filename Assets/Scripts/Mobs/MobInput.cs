using System.Collections.Generic;
using Cards;
using Managers;
using UnityEngine;

namespace Mobs
{
    public class MobInput : MobComponent
    {
        private void OnMouseEnter()
        {
            if (!ParentMob.IsHostile && ParentMob.State == MobState.Idle && !GameManager.Instance.ControlLock && GameManager.Instance.CurrentPhase == GamePhase.AssignActions)
            {
                if (ParentMob.UI.MobCursor)
                {
                    ParentMob.UI.MobCursor.Show();
                    UIManager.Instance.UISounds.ButtonHover();
                }
            }

            if (ParentMob.IsHostile && GameManager.Instance.ControlLock && ParentMob.UI.MobCursor)
            {
                ParentMob.UI.MobCursor.ZoomIn();
            }
        }

        private void OnMouseExit()
        {
            if (ParentMob.State == MobState.Idle && !GameManager.Instance.ControlLock && GameManager.Instance.CurrentPhase == GamePhase.AssignActions)
            {
                ParentMob.UI.MobCursor.Hide();
            }

            if (GameManager.Instance.ControlLock)
            {
                ParentMob.UI.MobCursor.ZoomOut();
            }
        }

        private void OnMouseOver()
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (ParentMob.State == MobState.Idle && !ParentMob.IsHostile && !GameManager.Instance.ControlLock && GameManager.Instance.CurrentPhase == GamePhase.AssignActions)
                {
                    if (ParentMob.State == MobState.Activated)
                    {
                        ParentMob.Deactivate();
                    }
                    else
                    {
                        ParentMob.Activate();
                    }
                }

                // Выбираем врагов
                if (ParentMob.IsHostile 
                    && GameManager.Instance.ControlLock
                    && GameManager.Instance.SelectingState == SelectingState.Enemy
                    && GameManager.Instance.PickingMob.CurrentAction.Targets.Count < GameManager.Instance.PickingMob.MobData.MaxTargets)
                {
                    PickEnemyMob();
                }
                
                // Выбираем союзников
                if (!ParentMob.IsHostile 
                    && GameManager.Instance.ControlLock
                    && GameManager.Instance.SelectingState == SelectingState.Player
                    && GameManager.Instance.PickingMob.CurrentAction.Targets.Count < GameManager.Instance.PickingMob.MobData.MaxTargets)
                {
                    PickPlayerMob();
                }
            }
        }

        private void PickEnemyMob()
        {
            ParentMob.UI.MobCursor.PickTarget();
            GameManager.Instance.PickingMob.CurrentAction.Targets.Add(ParentMob);
            if (GameManager.Instance.PickingMob.CurrentAction.MobActionType == ActionType.Attack)
            {
                CompletePicking(MobManager.Instance.EnemyMobs);
                return;
            }
            
            if (GameManager.Instance.PickingMob.CurrentAction.Targets.Count < GameManager.Instance.PickingMob.MobData.MaxTargets) return;
            CompletePicking(MobManager.Instance.EnemyMobs);
        }

        private void PickPlayerMob()
        {
            ParentMob.UI.MobCursor.PickTarget();
            GameManager.Instance.PickingMob.CurrentAction.Targets.Add(ParentMob);
            
            if (GameManager.Instance.PickingMob.CurrentAction.Targets.Count < GameManager.Instance.PickingMob.MobData.MaxTargets) return;
            
            CompletePicking(MobManager.Instance.PlayerMobs);
        }

        private void CompletePicking(List<Mob> mobList)
        {
            GameManager.Instance.PickingMob.MobActions.ActionPrepared();
            GameManager.Instance.PickingMob.Deactivate();
            GameManager.Instance.PickingMob.CurrentCombo = CardPanel.Instance.GetCombo();
                
            foreach (Mob mob in mobList)
            {
                if (mob != ParentMob) mob.Deactivate();
            }
            GameManager.Instance.ControlLock = false;
            GameManager.Instance.SelectingState = SelectingState.None;
            CardPanel.Instance.DisableInteraction();
        }
    }
}
