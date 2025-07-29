using System;
using Mobs;
using UnityEngine;

[CreateAssetMenu (fileName = "Status Effect Immune", menuName = "Data/Skills/Passive Skills/Status Effect Immune", order = 1)]
public class StatusEffectImmuneSkill : PassiveSkill
{
    private void Reset()
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
