using System;
using Mobs;
using Mobs.Skills;
using UnityEngine;

public class ReflectDamageSkill : PassiveSkill
{
    private void Start()
    {
        SkillName = "Reflect Damage";
        IsAttack = true;
        IsRanged = true;
    }

    private void OnEnable()
    {
        MobActions.OnDeflect += Use;
    }

    private void OnDisable()
    {
        MobActions.OnDeflect -= Use;
    }

    public void Use(Mob targetMob, float damageAmount)
    {
        if (ParentMob)
        {
            targetMob.MobCombatSystem.GetDamage(damageAmount, targetMob.CurrentCombo);
        }
    }

    public override void Use(Mob targetMob)
    {
        throw new System.NotImplementedException();
    }
}
