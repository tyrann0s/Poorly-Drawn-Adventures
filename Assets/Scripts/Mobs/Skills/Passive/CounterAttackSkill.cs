using System;
using System.Collections;
using Managers;
using Mobs;
using Mobs.Skills;
using UnityEngine;

public class CounterAttackSkill : PassiveSkill
{
    private void Start()
    {
        SkillName = "Counter Attack";
        IsAttack = true;
        IsRanged = false;
    }

    private void OnEnable()
    {
        MobActions.OnDamage += Use;
    }

    private void OnDisable()
    {
        MobActions.OnDamage -= Use;
    }

    private void Use(Mob targetMob, float damage, bool isMelee)
    {
        if (ParentMob)
        {
            if (ParentMob.State == MobState.Dead) return;
            if (isMelee) targetMob.MobCombatSystem.GetDamage(Amount, null);
        }
    }

    public override void Use(Mob targetMob)
    {
        
    }
}
