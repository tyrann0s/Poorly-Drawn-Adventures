using Mobs;
using UnityEngine;

namespace Items.Scrolls
{
    public class AttackScroll : ItemScroll
    {
        public override void Initialize(ElementType damageType, bool isAOE, Mob target)
        {
            base.Initialize(damageType, isAOE, target);
        }

        public override void Use()
        {
            base.Use();
        }
    }
}
