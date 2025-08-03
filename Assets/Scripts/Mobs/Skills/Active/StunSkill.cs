using Mobs;
using Mobs.Skills;
using Mobs.Status_Effects;
using UnityEngine;

public class StunSkill : ActiveSkill
{
    public override void Initialize(Mob parentMob, float amount, float cost, int duration = 0)
    {
        base.Initialize(parentMob, amount, cost, 1);
        
        SkillName = "Stun Skill";
        IsAttack = true;
        IsRanged = false;

        Duration = duration;
    }
    
    public override void Use(Mob targetMob)
    {
        targetMob.MobCombatSystem.GetDamage(Amount, ParentMob.CurrentCombo);
        targetMob.MobStatusEffects.AddEffect(StatusEffectType.Stun, Duration);
    }
}
