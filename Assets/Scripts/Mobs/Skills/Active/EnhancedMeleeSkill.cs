using Mobs;
using Mobs.Skills;
using UnityEngine;

public class EnhancedMeleeSkill : ActiveSkill
{
    private void Start()
    {
        SkillName = "Enhacned Melee Skill";
        
        IsAttack = true;
        IsRanged = false;
    }
    
    public override void Use(Mob targetMob)
    {
        targetMob.MobCombatSystem.GetDamage(Amount, ParentMob.CurrentCombo);
    }
}
