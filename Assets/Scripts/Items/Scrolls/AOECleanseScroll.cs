using Managers;
using UnityEngine;

namespace Items.Scrolls
{
    public class AOECleanseScroll : ScrollItem
    {
        public override void Use()
        {
            base.Use();
            foreach (var mob in ServiceLocator.Get<MobManager>().PlayerMobs)
            {
                mob.MobStatusEffects.StatusEffects.RemoveAll(x => x.IsNegative);
                mob.UI.ShowText("Cleansed!", Color.green);
            }
        }
    }
}
