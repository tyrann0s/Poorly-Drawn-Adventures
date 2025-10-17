using System.Linq;
using Cards;
using Managers;
using UnityEngine;

namespace Items.Scrolls
{
    public class AOEAttackScroll : ScrollItem
    {
        [SerializeField] private float damageAmount;
        [SerializeField] private ElementType damageType;
        [SerializeField] private bool ignoreShield;
    
        public override void Use()
        {
            base.Use();
            
            var mob = ServiceLocator.Get<MobManager>().EnemyMobs.FirstOrDefault(item => item);
            var combo = new ElementCombo(damageType, false, 0, ignoreShield, true);
            mob.MobCombatSystem.GetDamage(damageAmount, combo);
        }
    }
}
