using Mobs;
using Mobs.Skills;
using UnityEngine;

public class CleanseSkill : ActiveSkill
{
    public override void Initialize(Mob parentMob, float amount, float cost)
    {
        base.Initialize(parentMob, amount, cost);
        
        SkillName = "Cleanse";
        IsAttack = false;
        IsRanged = true;
    }
    
    public override void Use(Mob targetMob)
    {
        targetMob.MobStatusEffects.StatusEffects.RemoveAll(x => x.IsNegative);
        targetMob.UI.ShowText("Cleansed!", Color.green);
    }
}
