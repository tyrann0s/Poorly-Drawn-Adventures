using Mobs;
using Mobs.Skills;
using UnityEngine;

public class ResurrectSkill : ActiveSkill
{
    public override void Initialize(Mob parentMob, float amount, float cost, int duration = 0)
    {
        base.Initialize(parentMob, amount, cost, 1);
        
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
