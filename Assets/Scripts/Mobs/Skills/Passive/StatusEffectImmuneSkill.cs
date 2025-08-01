using System;
using Mobs;
using Mobs.Skills;
using UnityEngine;

public class StatusEffectImmuneSkill : PassiveSkill
{
    public override void Initialize(Mob parentMob, float amount, float cost, int duration = 0)
    {
        base.Initialize(parentMob, amount, cost, 1);
        
        SkillName = "Status Effect Immune";
        IsAttack = false;
        IsRanged = true;
    }

    public override void Use(Mob targetMob)
    {
        Debug.Log($"{ParentMob} immune to status effects!");
    }
}
