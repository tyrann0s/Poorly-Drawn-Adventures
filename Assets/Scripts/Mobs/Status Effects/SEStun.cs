using UnityEngine;

namespace Mobs.Status_Effects
{
    public class SEStun : StatusEffect
    {
        public SEStun()
        {
            IsNegative = true;
        }
        
        public override void ApplyEffect(Mob parentMob)
        {
            parentMob.UI.ShowText("Stunned!", Color.blue);
        }
    }
}
