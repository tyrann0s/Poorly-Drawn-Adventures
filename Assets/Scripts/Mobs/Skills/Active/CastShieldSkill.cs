using Mobs;
using Mobs.Skills;
using Mobs.Status_Effects;
using UnityEngine;

public class CastShieldSkill : ActiveSkill
{
    public override void Initialize(Mob parentMob, float amount, float cost)
    {
        base.Initialize(parentMob, amount, cost);
        
        SkillName = "Cast Shield";
        IsAttack = false;
        IsRanged = true;
    }
    
    public override void Use(Mob targetMob)
    {
        targetMob.MobStatusEffects.AddEffect(StatusEffectType.Defense, 1);
    }
}
