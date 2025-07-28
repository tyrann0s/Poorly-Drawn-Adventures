using Managers;
using UnityEngine;
namespace Mobs.Status_Effects
{
    public class SEDefense : StatusEffect
    {
        public SEDefense()
        {
            IsNegative = false;
        }
        
        public override void ApplyEffect(Mob parentMob)
        {
            parentMob.UI.ShowText("Defense!", Color.blue);
            ServiceLocator.Get<UIManager>().UISounds.ShieldActivation();
            parentMob.UI.ShowShield();
        }
    }
}
