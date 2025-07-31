using System;
using Mobs;
using Mobs.Skills;
using UnityEngine;

public class ReflectDamageSkill : PassiveSkill
{
    public override void Initialize(Mob parentMob, float amount, float cost)
    {
        base.Initialize(parentMob, amount, cost);
        
        SkillName = "Reflect Damage";
        IsAttack = true;
        IsRanged = true;
        
        MobActions.OnDeflect += Use;
    }

    public override void Cleanup()
    {
        MobActions.OnDeflect -= Use;
        base.Cleanup();
    }

    public void Use(Mob parentMob, Mob targetMob, float damageAmount)
    {
        if (parentMob != ParentMob) return;
        targetMob.MobCombatSystem.GetDamage(damageAmount, targetMob.CurrentCombo);
    }

    public override void Use(Mob targetMob)
    {
        throw new System.NotImplementedException();
    }
}
