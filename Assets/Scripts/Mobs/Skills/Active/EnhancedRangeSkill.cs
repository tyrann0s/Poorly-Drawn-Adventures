using Mobs;
using UnityEngine;

[ CreateAssetMenu (fileName = "Enhanced Range Skill", menuName = "Data/Skills/Active Skills/Enhanced Range Skill", order = 1)]
public class EnhancedRangeSkill : ActiveSkill
{
    private void Reset()
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
