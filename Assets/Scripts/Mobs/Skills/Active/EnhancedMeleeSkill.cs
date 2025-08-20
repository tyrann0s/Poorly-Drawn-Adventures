using Mobs;
using Mobs.Skills;
using UnityEngine;

public class EnhancedMeleeSkill : ActiveSkill
{
    public override void Initialize(Mob parentMob, float amount, float cost, int duration = 0)
    {
        base.Initialize(parentMob, amount, cost, 1);
        
        SkillName = "Enhacned Melee Skill";
        Description = "A skill that does more damage.";
        IsAttack = true;
        IsRanged = false;
    }
    
    public override void Use(Mob targetMob)
    {
        targetMob.MobCombatSystem.GetDamage(Amount, ParentMob.CurrentCombo);
    }
}
