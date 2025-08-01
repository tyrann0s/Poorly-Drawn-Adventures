using Mobs;
using Mobs.Skills;
using Mobs.Status_Effects;
using UnityEngine;

public class CastShieldSkill : ActiveSkill
{
    public override void Initialize(Mob parentMob, float amount, float cost, int duration = 0)
    {
        base.Initialize(parentMob, amount, cost, 1);
        
        SkillName = "Cast Shield";
        IsAttack = false;
        IsRanged = true;
    }
    
    public override void Use(Mob targetMob)
    {
        targetMob.MobStatusEffects.AddEffect(StatusEffectType.Defense, 1);
    }
}
