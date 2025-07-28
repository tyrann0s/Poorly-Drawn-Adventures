using System;
using Mobs;
using Mobs.Status_Effects;
using UnityEngine;

[CreateAssetMenu (fileName = "Cast Status Effect", menuName = "Data/Skills/Passive Skills/Cast Status Effect", order = 1)]
public class CastStatusEffectSkill : PassiveSkill
{
    [SerializeField] private int duration = 1;
    private void Reset()
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
