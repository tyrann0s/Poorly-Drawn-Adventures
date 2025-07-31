using Mobs;
using Mobs.Skills;
using UnityEngine;

public class ResurrectSkill : ActiveSkill
{
    public override void Initialize(Mob parentMob, float amount, float cost)
    {
        base.Initialize(parentMob, amount, cost);
        
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
