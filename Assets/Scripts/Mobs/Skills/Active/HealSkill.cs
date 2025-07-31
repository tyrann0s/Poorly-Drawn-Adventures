using System;
using Mobs;
using Mobs.Skills;
using UnityEngine;


public class HealSkill : ActiveSkill
{
    public override void Initialize(Mob parentMob, float amount, float cost)
    {
        base.Initialize(parentMob, amount, cost);
        
        SkillName = "Heal";
        IsAttack = false;
        IsRanged = true;
    }

    public override void Use(Mob targetMob)
    {
        targetMob.MobCombatSystem.Heal(Amount);
    }
}
