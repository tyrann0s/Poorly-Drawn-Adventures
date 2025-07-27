using System;
using Mobs;
using UnityEngine;


[ CreateAssetMenu (fileName = "Heal", menuName = "Data/Skills/Active Skills/Heal", order = 1)]
public class HealSkill : ActiveSkill
{
    private void Reset()
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
