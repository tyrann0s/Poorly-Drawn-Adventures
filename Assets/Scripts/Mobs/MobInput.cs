using System.Collections.Generic;
using System.Linq;
using Cards;
using Managers;
using UnityEngine;

namespace Mobs
{
    public class MobInput : MobComponent
    {
        private void OnMouseEnter()
        {
            var targetContext = ServiceLocator.Get<TargetManager>().CurrentContext;

            if (targetContext == null)
                return;

            if (targetContext.CanSelectTarget(ParentMob))
            {
                if (targetContext.CurrentSelectingState == SelectingState.Player)
                {
                    ParentMob.UI.ShowCursor();
                    ServiceLocator.Get<UIManager>().UISounds.ButtonHover();
                }
                else
                {
                    ParentMob.UI.ShowEnemyHighlight();
                }
            }
        }

        private void OnMouseExit()
        {
            var targetContext = ServiceLocator.Get<TargetManager>().CurrentContext;

            if (targetContext == null)
                return;

            if (targetContext.CanSelectTarget(ParentMob))
            {
                if (targetContext.CurrentSelectingState == SelectingState.Player)
                {
                    ParentMob.UI.HideCursor();
                }
                else
                {
                    ParentMob.UI.HideEnemyHighlight();
                }
            }
        }

        private void OnMouseOver()
        {
            if (Input.GetMouseButtonUp(0))
            {
                var targetContext = ServiceLocator.Get<TargetManager>().CurrentContext;

                if (targetContext == null)
                    return;
                
                if (targetContext.CanSelectTarget(ParentMob))
                {
                    if (targetContext.CurrentSelectingState == SelectingState.Player &&
                        targetContext.CurrentSourceType == SourceType.MobControl)
                    {
                        if (ParentMob.State == MobState.Activated)
                        {
                            ParentMob.Deactivate();
                        }
                        else
                        {
                            ParentMob.Activate();
                        }
                        return;
                    }
                    
                    if (targetContext.CurrentSelectingState == SelectingState.Enemy && targetContext.MaxTargets >= ServiceLocator.Get<TargetManager>().Targets.Count)
                    {
                        ServiceLocator.Get<TargetManager>().AddTarget(ParentMob);
                        PickEnemyMob();
                        ParentMob.UI.HideEnemyHighlight();
                        return;
                    } 
                    
                    if (targetContext.CurrentSelectingState == SelectingState.Player && targetContext.MaxTargets >= ServiceLocator.Get<TargetManager>().Targets.Count)
                    {
                        ServiceLocator.Get<TargetManager>().AddTarget(ParentMob);
                        if (ServiceLocator.Get<GameManager>().PickingMob.MobData.ActiveSkill is ResurrectSkill)
                        {
                            if (ParentMob.State == MobState.Dead) PickPlayerMob();
                        } else if (ParentMob.State != MobState.Dead) PickPlayerMob();
                    } 
                }
            }
        }

        private void PickEnemyMob()
        {
            ServiceLocator.Get<GameManager>().PickingMob.CurrentAction.Targets.Add(ParentMob);
            if (ServiceLocator.Get<GameManager>().PickingMob.CurrentAction.MobActionType == ActionType.Attack)
            {
                ParentMob.UI.PickTarget(true);
                CompletePicking(ServiceLocator.Get<MobManager>().EnemyMobs);
                return;
            }

            if (ServiceLocator.Get<GameManager>().PickingMob.CurrentAction.Targets.Count <
                ServiceLocator.Get<GameManager>().PickingMob.MobData.MaxTargets)
            {
                ParentMob.UI.PickTarget(false);
                return;
            }
            
            ParentMob.UI.PickTarget(true);
            CompletePicking(ServiceLocator.Get<MobManager>().EnemyMobs);
        }

        private void PickPlayerMob()
        {
            ServiceLocator.Get<GameManager>().PickingMob.CurrentAction.Targets.Add(ParentMob);

            if (ServiceLocator.Get<GameManager>().PickingMob.CurrentAction.Targets.Count <
                ServiceLocator.Get<GameManager>().PickingMob.MobData.MaxTargets)
            {
                ParentMob.UI.PickTarget(false);
                return;
            }
            
            ParentMob.UI.PickTarget(true);
            CompletePicking(ServiceLocator.Get<MobManager>().PlayerMobs);
        }

        private void CompletePicking(List<Mob> mobList)
        {
            ServiceLocator.Get<GameManager>().PickingMob.MobActions.ActionPrepared();
            ServiceLocator.Get<GameManager>().PickingMob.Deactivate();
            ServiceLocator.Get<GameManager>().PickingMob.CurrentCombo = ServiceLocator.Get<CardPanel>().GetCombo();
            ServiceLocator.Get<CardPanel>().HideCurrentCombo();
                
            foreach (Mob mob in mobList)
            {
                if (mob != ParentMob) mob.Deactivate();
                mob.UI.HideCursor();
            }
            
            ServiceLocator.Get<GameManager>().ControlLock = false;
            ServiceLocator.Get<CardPanel>().DisableInteraction();
            if (ServiceLocator.Get<MobManager>().PlayerMobs.Any(mob => mob.State == MobState.Idle))
            {
                ServiceLocator.Get<TargetManager>().SetContext(new TargetSelectionContext(
                    SourceType.MobControl, 
                    SelectingState.Player, 
                    null,
                    null, 
                    mob => !mob.IsHostile && mob.State == MobState.Idle));
            } else ServiceLocator.Get<TargetManager>().ClearContext();
        }
    }
}
