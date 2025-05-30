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

                if (ParentMob.IsHostile && GameManager.Instance.ControlLock)
                {
                    ParentMob.UI.MobCursor.PickTarget();
                    GameManager.Instance.PickingMob.CurrentAction.TargetInstance = ParentMob;
                    GameManager.Instance.PickingMob.MobActions.ActionPrepared();
                    GameManager.Instance.PickingMob.Deactivate();
                    GameManager.Instance.PickingMob.CurrentCombo = GameManager.Instance.GetCombo();
                
                    foreach (Mob mob in GameManager.Instance.EnemyMobs)
                    {
                        if (mob != ParentMob) mob.Deactivate();
                    }
                    GameManager.Instance.ControlLock = false;
                    GameManager.Instance.SetCardPanel(false);
                }
            }
        }
    }
}
