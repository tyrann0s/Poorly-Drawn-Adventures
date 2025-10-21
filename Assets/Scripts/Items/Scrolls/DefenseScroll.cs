using Managers;
using Mobs;
using Mobs.Status_Effects;
using UI.Inventory;
using UnityEngine;

namespace Items.Scrolls
{
    public class DefenseScroll : ScrollItem
    {
        public override void Initialize(Item item, Sprite icon, ItemScroll scroll)
        {
            base.Initialize(item, icon, scroll);
            
            ServiceLocator.Get<TargetManager>().SetContext(new TargetSelectionContext(
                SourceType.ItemTarget,
                SelectingState.Player, 
                mob => !mob.IsHostile && mob.State == MobState.Idle));
        }

        public override void Use()
        {
            if (ServiceLocator.Get<TargetManager>().Targets.Count > 0)
            {
                foreach (var mob in ServiceLocator.Get<TargetManager>().Targets)
                {
                    mob.MobStatusEffects.AddEffect(StatusEffectType.Defense, 1);
                    mob.UI.HideCursor();
                }
                base.Use();
            }
        }
    }
}
