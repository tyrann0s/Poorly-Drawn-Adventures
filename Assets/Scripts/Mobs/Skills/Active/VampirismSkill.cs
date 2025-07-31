using Mobs;
using Mobs.Skills;
using UnityEngine;

public class VampirismSkill : ActiveSkill
{
    public override void Initialize(Mob parentMob, float amount, float cost)
    {
        base.Initialize(parentMob, amount, cost);
        
        SkillName = "Vampirism";
        IsAttack = true;
        IsRanged = true;
    }
    
    public override void Use(Mob targetMob)
    {
        targetMob.MobCombatSystem.GetDamage(Amount, ParentMob.CurrentCombo);
        if (!targetMob.MobCombatSystem.HandleShieldAttack(ParentMob.CurrentCombo)) ParentMob.MobCombatSystem.Heal(Amount);
    }
}
