using UnityEngine;

namespace Mobs.Status_Effects
{
    public class SEStun : StatusEffect
    {
        public SEStun()
        {
            IsNegative = true;
        }
        
        public override void ApplyEffect()
        {
            ParentMob.UI.ShowText("Stunned!", Color.blue);
        }
    }
}
