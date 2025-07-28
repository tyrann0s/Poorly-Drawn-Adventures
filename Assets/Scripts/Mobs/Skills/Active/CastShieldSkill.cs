using Mobs;
using Mobs.Status_Effects;
using UnityEngine;

[ CreateAssetMenu (fileName = "Cast Shield", menuName = "Data/Skills/Active Skills/Cast Shield", order = 1)]
public class CastShieldSkill : ActiveSkill
{
    private void Reset()
    {
        SkillName = "Cast Shield";
        
        IsAttack = false;
        IsRanged = true;
    }
    
    public override void Use(Mob targetMob)
    {
        targetMob.MobStatusEffects.AddEffect(StatusEffectType.Defense, 1);
    }
}
