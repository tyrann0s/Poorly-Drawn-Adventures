using Mobs;
using UnityEngine;

[ CreateAssetMenu (fileName = "Enhanced Melee Skill", menuName = "Data/Skills/Active Skills/Enhanced Melee Skill", order = 1)]
public class EnhancedMeleeSkill : ActiveSkill
{
    private void Reset()
    {
        SkillName = "Enhacned Melee Skill";
        
        IsAttack = true;
        IsRanged = false;
    }
    
    public override void Use(Mob parentMob, Mob targetMob)
    {
        base.Use(parentMob, targetMob);
        targetMob.MobCombatSystem.GetDamage(Amount, targetMob.CurrentCombo);
    }
}
