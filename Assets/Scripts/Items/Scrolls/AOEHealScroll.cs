using Managers;
using UI.Inventory;
using UnityEngine;

namespace Items.Scrolls
{
    public class AOEHealScroll : ScrollItem
    {
        [SerializeField] private float healAmount;
    
        public override void Use()
        {
            base.Use();
            foreach (var mob in ServiceLocator.Get<MobManager>().PlayerMobs)
            {
                mob.MobCombatSystem.Heal(healAmount);
            }
        }
    }
}
