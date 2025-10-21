using Managers;
using Mobs;
using UI.Inventory;
using UnityEngine;

namespace Items.Scrolls
{
    public class ReviveScroll : ScrollItem
    {
        public override void Initialize(Item item, Sprite icon, ItemScroll scroll)
        {
            base.Initialize(item, icon, scroll);
            
            ServiceLocator.Get<TargetManager>().SetContext(new TargetSelectionContext(
                SourceType.ItemTarget,
                SelectingState.Player, 
                mob => !mob.IsHostile && mob.State == MobState.Dead));
        }

        public override void Use()
        {
            if (ServiceLocator.Get<TargetManager>().Targets.Count > 0)
            {
                foreach (var mob in ServiceLocator.Get<TargetManager>().Targets)
                {
                    mob.MobCombatSystem.Revive();
                    mob.UI.HideCursor();
                }
                base.Use();
            }
        }
    }
}
