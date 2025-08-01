using System;
using System.Collections;
using Managers;
using Mobs;
using Mobs.Skills;
using UnityEngine;

public class CounterAttackSkill : PassiveSkill
{
    public override void Initialize(Mob parentMob, float amount, float cost, int duration = 0)
    {
        base.Initialize(parentMob, amount, cost, 1);
        
        SkillName = "Counter Attack";
        IsAttack = true;
        IsRanged = false;
        
        MobActions.OnDamage += Use;
    }

    public override void Cleanup()
    {
        MobActions.OnDamage -= Use;
        base.Cleanup();
    }

    public void Use(Mob parentMob, Mob targetMob, float damageAmount, bool isMelee)
    {
        if (parentMob != ParentMob) return;
        if (ParentMob.State == MobState.Dead) return;
        if (isMelee) targetMob.MobCombatSystem.GetDamage(Amount, null);
    }

    public override void Use(Mob targetMob)
    {
        
    }
}
