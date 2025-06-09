using Managers;
using UnityEngine;
namespace Mobs.Status_Effects
{
    public class SEDefense : StatusEffect
    {
        public override void ApplyEffect()
        {
            ParentMob.UI.ShowText("Defense!", Color.blue);
            UIManager.Instance.UISounds.ShieldActivation();
            ParentMob.UI.ShowShield();
        }
    }
}
