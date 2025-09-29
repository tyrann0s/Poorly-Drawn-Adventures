using System;
using Mobs;
using Mobs.Skills;
using UnityEngine;

public class ReflectDamageSkill : PassiveSkill
{
    public override void Initialize(Mob parentMob, float amount, float cost, int duration = 0)
    {
        base.Initialize(parentMob, amount, cost, 1);
        
        SkillName = "Reflect Damage";
        IsAttack = true;
        IsRanged = true;
        
        MobActions.OnDeflect += Use;
    }

    protected override void Cleanup()
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
