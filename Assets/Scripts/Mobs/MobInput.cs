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
                ParentMob.UI.ShowCursor();
                ServiceLocator.Get<UIManager>().UISounds.ButtonHover();
            }

            if (ParentMob.IsHostile && ServiceLocator.Get<GameManager>().SelectingState == SelectingState.Enemy)
            {
                ParentMob.UI.ShowEnemyHighlight();
            }
        }

        private void OnMouseExit()
        {
            if (!ParentMob.IsHostile && ParentMob.State == MobState.Idle && ServiceLocator.Get<GameManager>().CurrentPhase == GamePhase.AssignActions)
            {
                ParentMob.UI.HideCursor();
            }

            if (ParentMob.IsHostile && ServiceLocator.Get<GameManager>().SelectingState == SelectingState.Enemy)
            {
                ParentMob.UI.HideEnemyHighlight();
            }
        }

        private void OnMouseOver()
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (ParentMob.State == MobState.Idle && !ParentMob.IsHostile &&
                    !ServiceLocator.Get<GameManager>().ControlLock &&
                    ServiceLocator.Get<GameManager>().CurrentPhase == GamePhase.AssignActions)
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
                    ParentMob.UI.HideEnemyHighlight();
                }
                
                // Выбираем союзников
                if (!ParentMob.IsHostile 
                    && ServiceLocator.Get<GameManager>().ControlLock
                    && ServiceLocator.Get<GameManager>().SelectingState == SelectingState.Player
                    && ServiceLocator.Get<GameManager>().PickingMob.CurrentAction.Targets.Count < ServiceLocator.Get<GameManager>().PickingMob.MobData.MaxTargets)
                {
                    if (ServiceLocator.Get<GameManager>().PickingMob.MobData.ActiveSkill is ResurrectSkill)
                    {
                        if (ParentMob.State == MobState.Dead) PickPlayerMob();
                    } else if (ParentMob.State != MobState.Dead) PickPlayerMob();
                }
            }
        }

        private void PickEnemyMob()
        {
            ParentMob.UI.PickTarget();
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
            ParentMob.UI.PickTarget();
            ServiceLocator.Get<GameManager>().PickingMob.CurrentAction.Targets.Add(ParentMob);
            
            if (ServiceLocator.Get<GameManager>().PickingMob.CurrentAction.Targets.Count < ServiceLocator.Get<GameManager>().PickingMob.MobData.MaxTargets) return;
            
            CompletePicking(ServiceLocator.Get<MobManager>().PlayerMobs);
        }

        private void CompletePicking(List<Mob> mobList)
        {
            ServiceLocator.Get<GameManager>().PickingMob.MobActions.ActionPrepared();
            ServiceLocator.Get<GameManager>().PickingMob.Deactivate();
            ServiceLocator.Get<GameManager>().PickingMob.CurrentCombo = ServiceLocator.Get<CardPanel>().GetCombo();
            ServiceLocator.Get<UIManager>().ShowCombination("");
                
            foreach (Mob mob in mobList)
            {
                if (mob != ParentMob) mob.Deactivate();
                mob.UI.HideCursor();
            }
            
            ServiceLocator.Get<GameManager>().ControlLock = false;
            ServiceLocator.Get<GameManager>().SelectingState = SelectingState.None;
            ServiceLocator.Get<CardPanel>().DisableInteraction();
        }
    }
}
