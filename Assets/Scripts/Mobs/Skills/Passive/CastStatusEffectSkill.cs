using Mobs;
using Mobs.Skills;
using UnityEngine;

public class CastStatusEffectSkill : PassiveSkill
{
    [SerializeField] private int duration = 1;

    public override void Initialize(Mob parentMob, float amount, float cost)
    {
        base.Initialize(parentMob, amount, cost);
        
        SkillName = "Cast Status Effect";
        IsAttack = false;
        IsRanged = true;
        
        MobActions.OnAttack += Use;
    }

    public override void Cleanup()
    {
        MobActions.OnAttack -= Use;
        base.Cleanup();
    }

    public void Use(Mob parentMob, Mob targetMob)
    {
        if (parentMob == ParentMob)
        {
            targetMob.MobStatusEffects.AddEffect(ParentMob.MobData.AttackElement, duration);
        }
    }
    
    public override void Use(Mob targetMob)
    {
        throw new System.NotImplementedException();
    }
}
