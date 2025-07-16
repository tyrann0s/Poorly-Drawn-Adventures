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
            if (!ParentMob.IsHostile &&
                !ServiceLocator.Get<GameManager>().ControlLock &&
                ParentMob.State == MobState.Idle &&
                ServiceLocator.Get<GameManager>().CurrentPhase == GamePhase.AssignActions)
            {
                if (ParentMob.UI.MobCursor)
                {
                    ParentMob.UI.MobCursor.Show();
                    ServiceLocator.Get<UIManager>().UISounds.ButtonHover();
                }
            }

            if (ParentMob.IsHostile && ServiceLocator.Get<GameManager>().SelectingState == SelectingState.Enemy)
            {
                ParentMob.UI.MobCursor.ZoomIn();
            }
        }

        private void OnMouseExit()
        {
            if (!ParentMob.IsHostile && ParentMob.State == MobState.Idle && ServiceLocator.Get<GameManager>().CurrentPhase == GamePhase.AssignActions)
            {
                ParentMob.UI.MobCursor.Hide();
            }

            if (ParentMob.IsHostile && ServiceLocator.Get<GameManager>().SelectingState == SelectingState.Enemy)
            {
                ParentMob.UI.MobCursor.ZoomOut();
            }
        }

        private void OnMouseOver()
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (ParentMob.State == MobState.Idle && !ParentMob.IsHostile && !ServiceLocator.Get<GameManager>().ControlLock && ServiceLocator.Get<GameManager>().CurrentPhase == GamePhase.AssignActions)
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
                    && ServiceLocator.Get<GameManager>().ControlLock
                    && ServiceLocator.Get<GameManager>().SelectingState == SelectingState.Enemy
                    && ServiceLocator.Get<GameManager>().PickingMob.CurrentAction.Targets.Count < ServiceLocator.Get<GameManager>().PickingMob.MobData.MaxTargets)
                {
                    PickEnemyMob();
                }
                
                // Выбираем союзников
                if (!ParentMob.IsHostile 
                    && ServiceLocator.Get<GameManager>().ControlLock
                    && ServiceLocator.Get<GameManager>().SelectingState == SelectingState.Player
                    && ServiceLocator.Get<GameManager>().PickingMob.CurrentAction.Targets.Count < ServiceLocator.Get<GameManager>().PickingMob.MobData.MaxTargets)
                {
                    PickPlayerMob();
                }
            }
        }

        private void PickEnemyMob()
        {
            ParentMob.UI.MobCursor.PickTarget();
            ServiceLocator.Get<GameManager>().PickingMob.CurrentAction.Targets.Add(ParentMob);
            if (ServiceLocator.Get<GameManager>().PickingMob.CurrentAction.MobActionType == ActionType.Attack)
            {
                CompletePicking(ServiceLocator.Get<MobManager>().EnemyMobs);
                return;
            }
            
            if (ServiceLocator.Get<GameManager>().PickingMob.CurrentAction.Targets.Count < ServiceLocator.Get<GameManager>().PickingMob.MobData.MaxTargets) return;
            CompletePicking(ServiceLocator.Get<MobManager>().EnemyMobs);
        }

        private void PickPlayerMob()
        {
            ParentMob.UI.MobCursor.PickTarget();
            ServiceLocator.Get<GameManager>().PickingMob.CurrentAction.Targets.Add(ParentMob);
            
            if (ServiceLocator.Get<GameManager>().PickingMob.CurrentAction.Targets.Count < ServiceLocator.Get<GameManager>().PickingMob.MobData.MaxTargets) return;
            
            CompletePicking(ServiceLocator.Get<MobManager>().PlayerMobs);
        }

        private void CompletePicking(List<Mob> mobList)
        {
            ServiceLocator.Get<GameManager>().PickingMob.MobActions.ActionPrepared();
            ServiceLocator.Get<GameManager>().PickingMob.Deactivate();
            ServiceLocator.Get<GameManager>().PickingMob.CurrentCombo = ServiceLocator.Get<CardPanel>().GetCombo();
                
            foreach (Mob mob in mobList)
            {
                if (mob != ParentMob) mob.Deactivate();
            }
            ServiceLocator.Get<GameManager>().ControlLock = false;
            ServiceLocator.Get<GameManager>().SelectingState = SelectingState.None;
            ServiceLocator.Get<CardPanel>().DisableInteraction();
        }
    }
}
