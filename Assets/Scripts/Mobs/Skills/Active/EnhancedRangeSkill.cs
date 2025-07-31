using Mobs;
using Mobs.Skills;
using UnityEngine;

public class EnhancedRangeSkill : ActiveSkill
{
    public override void Initialize(Mob parentMob, float amount, float cost)
    {
        base.Initialize(parentMob, amount, cost);
        
        SkillName = "Enhanced Range Skill";
        IsAttack = true;
        IsRanged = true;
    }
    
    public override void Use(Mob targetMob)
    {
        targetMob.MobCombatSystem.GetDamage(Amount, ParentMob.CurrentCombo);
    }
}
