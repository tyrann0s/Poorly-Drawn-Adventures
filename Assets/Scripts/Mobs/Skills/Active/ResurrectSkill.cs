using Mobs;
using UnityEngine;

[ CreateAssetMenu (fileName = "Resurrect", menuName = "Data/Skills/Active Skills/Resurrect", order = 1)]
public class ResurrectSkill : ActiveSkill
{
    private void Reset()
    {
        SkillName = "Resurrect";
        
        IsAttack = false;
        IsRanged = true;
    }
    
    public override void Use(Mob targetMob)
    {
        base.Use(targetMob);

        if (targetMob.State == MobState.Dead)
        {
            targetMob.MobCombatSystem.Revive();
        }
    }
}
