using Managers;
using UnityEngine;
namespace Mobs.Status_Effects
{
    public class SEDefense : StatusEffect
    {
        public override void ApplyEffect()
        {
            ParentMob.UI.ShowText("Defense!", Color.blue);
            ServiceLocator.Get<UIManager>().UISounds.ShieldActivation();
            ParentMob.UI.ShowShield();
        }
    }
}
