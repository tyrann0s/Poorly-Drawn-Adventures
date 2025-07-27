using Mobs;
using UnityEngine;

[ CreateAssetMenu (fileName = "Vampirism", menuName = "Data/Skills/Active Skills/Vampirism", order = 1)]
public class VampirismSkill : ActiveSkill
{
    private void Reset()
    {
        SkillName = "Vampirism";
        
        IsAttack = true;
        IsRanged = true;
    }
    
    public override void Use(Mob targetMob)
    {
        targetMob.MobCombatSystem.GetDamage(Amount, targetMob.CurrentCombo);
        if (!targetMob.MobCombatSystem.HandleShieldAttack(targetMob.CurrentCombo)) ParentMob.MobCombatSystem.Heal(Amount);
    }
}
