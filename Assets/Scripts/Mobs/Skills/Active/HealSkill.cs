using System;
using Mobs;
using Mobs.Skills;
using UnityEngine;


public class HealSkill : ActiveSkill
{
    public override void Initialize(Mob parentMob, float amount, float cost, int duration = 0)
    {
        base.Initialize(parentMob, amount, cost, 1);
        
        SkillName = "Heal";
        Description = "Heals the mob for a certain amount.";
        IsAttack = false;
        IsRanged = true;
    }

    public override void Use(Mob targetMob)
    {
        targetMob.MobCombatSystem.Heal(Amount);
    }
}
