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
        base.Use(targetMob);
        if (targetMob.State == MobState.Dead) return;
        targetMob.MobHP += Amount;
        if (targetMob.MobHP > targetMob.MobData.MaxHP) targetMob.MobHP = targetMob.MobData.MaxHP;
        targetMob.UI.UpdateHP(targetMob.MobHP);
        targetMob.UI.ShowText("Healed!", Color.green);
    }
}
