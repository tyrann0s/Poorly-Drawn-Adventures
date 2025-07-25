using Mobs;
using UnityEngine;

[ CreateAssetMenu (fileName = "Cleanse", menuName = "Data/Skills/Active Skills/Cleanse", order = 1)]
public class CleanseSkill : ActiveSkill
{
    private void Reset()
    {
        SkillName = "Cleanse";
        
        IsAttack = false;
        IsRanged = true;
    }
    
    public override void Use(Mob parentMob, Mob targetMob)
    {
        base.Use(parentMob, targetMob);
        targetMob.MobStatusEffects.StatusEffects.RemoveAll(x => x.IsNegative);
        targetMob.UI.ShowText("Cleansed!", Color.green);
    }
}
