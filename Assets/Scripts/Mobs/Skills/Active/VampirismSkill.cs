using Mobs;
using Mobs.Skills;
using UnityEngine;

public class VampirismSkill : ActiveSkill
{
    private void Start()
    {
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
