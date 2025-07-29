using System;
using Mobs;
using Mobs.Skills;
using UnityEngine;


public class HealSkill : ActiveSkill
{
    private void Start()
    {
        SkillName = "Heal";
        
        IsAttack = false;
        IsRanged = true;
    }

    public override void Use(Mob targetMob)
    {
        targetMob.MobCombatSystem.Heal(Amount);
    }
}
