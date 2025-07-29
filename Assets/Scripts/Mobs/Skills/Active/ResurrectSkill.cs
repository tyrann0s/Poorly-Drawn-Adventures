using Mobs;
using Mobs.Skills;
using UnityEngine;

public class ResurrectSkill : ActiveSkill
{
    private void Start()
    {
        SkillName = "Resurrect";
        
        IsAttack = false;
        IsRanged = true;
    }
    
    public override void Use(Mob targetMob)
    {
        if (targetMob.State == MobState.Dead)
        {
            targetMob.MobCombatSystem.Revive();
        }
    }
}
