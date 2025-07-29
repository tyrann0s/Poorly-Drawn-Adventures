using System;
using Mobs;
using Mobs.Skills;
using UnityEngine;

public class StatusEffectImmuneSkill : PassiveSkill
{
    private void Start()
    {
        SkillName = "Status Effect Immune";
        IsAttack = false;
        IsRanged = true;
    }

    public override void Use(Mob targetMob)
    {
        Debug.Log($"{ParentMob} immune to status effects!");
    }
}
