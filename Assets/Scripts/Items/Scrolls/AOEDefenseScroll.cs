using Managers;
using Mobs.Status_Effects;

namespace Items.Scrolls
{
    public class AOEDefenseScroll : ScrollItem
    {
        public override void Use()
        {
            base.Use();
            foreach (var mob in ServiceLocator.Get<MobManager>().PlayerMobs)
            {
                mob.MobStatusEffects.AddEffect(StatusEffectType.Defense, 1);
            }
        }
    }
}
