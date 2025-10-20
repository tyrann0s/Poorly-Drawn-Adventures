using System.Linq;
using Cards;
using Managers;
using Mobs;
using UI.Inventory;
using UnityEngine;

namespace Items.Scrolls
{
    public class AttackScroll : ScrollItem
    {
        [SerializeField] private float damageAmount;
        [SerializeField] private ElementType damageType;
        [SerializeField] private bool ignoreShield;
    
        public override void Initialize(Item item, Sprite icon, ItemScroll scroll)
        {
            base.Initialize(item, icon, scroll);
            
            ServiceLocator.Get<TargetManager>().SetContext(new TargetSelectionContext(
                SourceType.ItemTarget,
                SelectingState.Enemy, 
                mob => mob.IsHostile && mob.State == MobState.Idle));
        }
        
        public override void Use()
        {
            if (ServiceLocator.Get<TargetManager>().Targets.Count > 0)
            {
                var combo = new ElementCombo(damageType, false, 0, ignoreShield, false);
                foreach (var mob in ServiceLocator.Get<TargetManager>().Targets)
                {
                    mob.MobCombatSystem.GetDamage(damageAmount, combo);
                    mob.UI.HideEnemyHighlight();
                }
                base.Use();
            }
        }
    }
}
