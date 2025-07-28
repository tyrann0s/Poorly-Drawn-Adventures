using System;
using Mobs;
using UnityEngine;

[CreateAssetMenu (fileName = "Reflect Damage", menuName = "Data/Skills/Passive Skills/Reflect Damage", order = 1)]
public class ReflectDamageSkill : PassiveSkill
{
    private void Reset()
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
