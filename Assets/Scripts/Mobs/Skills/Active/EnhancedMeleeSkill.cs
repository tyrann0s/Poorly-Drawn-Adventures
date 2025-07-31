using Mobs;
using Mobs.Skills;
using UnityEngine;

public class EnhancedMeleeSkill : ActiveSkill
{
    public override void Initialize(Mob parentMob, float amount, float cost)
    {
        base.Initialize(parentMob, amount, cost);
        
        SkillName = "Enhacned Melee Skill";
        IsAttack = true;
        IsRanged = false;
    }
    
    public override void Use(Mob targetMob)
    {
        targetMob.MobCombatSystem.GetDamage(Amount, ParentMob.CurrentCombo);
    }
}
