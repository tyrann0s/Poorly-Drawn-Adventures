using Mobs;
using Mobs.Skills;
using UnityEngine;

public class CleanseSkill : ActiveSkill
{
    private void Start()
    {
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
