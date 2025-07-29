using System;
using Mobs;
using Mobs.Skills;
using Mobs.Status_Effects;
using UnityEngine;

public class CastStatusEffectSkill : PassiveSkill
{
    [SerializeField] private int duration = 1;
    
    private void Start()
    {
        SkillName = "Cast Status Effect";
        IsAttack = false;
        IsRanged = true;
    }

    private void OnEnable()
    {
        MobActions.OnAttack += Use;
    }

    private void OnDisable()
    {
        MobActions.OnAttack -= Use;
    }

    public override void Use(Mob targetMob)
    {
        if (ParentMob)
        {
            targetMob.MobStatusEffects.AddEffect(ParentMob.MobData.AttackElement, duration);
        }
    }
}
