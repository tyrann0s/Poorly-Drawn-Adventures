using Mobs;
using Mobs.Skills;
using UnityEngine;

public class EnhancedRangeSkill : ActiveSkill
{
    private void Start()
    {
        SkillName = "Enhanced Range Skill";
        
        IsAttack = true;
        IsRanged = true;
    }
    
    public override void Use(Mob targetMob)
    {
        targetMob.MobCombatSystem.GetDamage(Amount, ParentMob.CurrentCombo);
    }
}
