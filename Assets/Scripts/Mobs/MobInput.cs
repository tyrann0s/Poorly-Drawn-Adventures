using Managers;
using UnityEngine;

namespace Mobs
{
    public class MobInput : MobComponent
    {
        private void OnMouseEnter()
        {
            if (!ParentMob.IsHostile && ParentMob.State == MobState.Idle && !GameManager.Instance.ControlLock && !GameManager.Instance.ChangeCardMode)
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
            if (ParentMob.State == MobState.Idle && !GameManager.Instance.ControlLock && !GameManager.Instance.ChangeCardMode)
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
                if (ParentMob.State == MobState.Idle && !ParentMob.IsHostile && !GameManager.Instance.ControlLock && !GameManager.Instance.ChangeCardMode)
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
            
            if (GameManager.Instance.PickingMob.CurrentAction.Targets.Count < GameManager.Instance.PickingMob.MobData.MaxTargets) return;
            
            GameManager.Instance.PickingMob.MobActions.ActionPrepared();
            GameManager.Instance.PickingMob.Deactivate();
            GameManager.Instance.PickingMob.CurrentCombo = GameManager.Instance.GetCombo();
                
            foreach (Mob mob in GameManager.Instance.EnemyMobs)
            {
                if (mob != ParentMob) mob.Deactivate();
            }
            GameManager.Instance.ControlLock = false;
            GameManager.Instance.SelectingState = SelectingState.None;
            GameManager.Instance.SetCardPanel(false);
        }

        private void PickPlayerMob()
        {
            ParentMob.UI.MobCursor.PickTarget();
            GameManager.Instance.PickingMob.CurrentAction.Targets.Add(ParentMob);
            
            if (GameManager.Instance.PickingMob.CurrentAction.Targets.Count < GameManager.Instance.PickingMob.MobData.MaxTargets) return;
            
            GameManager.Instance.PickingMob.MobActions.ActionPrepared();
            GameManager.Instance.PickingMob.Deactivate();
            GameManager.Instance.PickingMob.CurrentCombo = GameManager.Instance.GetCombo();
                
            foreach (Mob mob in GameManager.Instance.PlayerMobs)
            {
                if (mob != ParentMob) mob.Deactivate();
            }
            GameManager.Instance.ControlLock = false;
            GameManager.Instance.SelectingState = SelectingState.None;
            GameManager.Instance.SetCardPanel(false);
        }
    }
}
