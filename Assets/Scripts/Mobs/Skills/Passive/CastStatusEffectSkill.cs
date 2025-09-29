using System;
using Mobs;
using Mobs.Skills;
using UnityEngine;

public class CastStatusEffectSkill : PassiveSkill
{
    public override void Initialize(Mob parentMob, float amount, float cost, int duration)
    {
        base.Initialize(parentMob, amount, cost, duration);
        
        SkillName = "Cast Status Effect";
        IsAttack = false;
        IsRanged = true;
        
        Duration = duration + 1;
        
        MobActions.OnAttack += Use;
    }

    protected override void Cleanup()
    {
        MobActions.OnAttack -= Use;
        base.Cleanup();
    }

    public void Use(Mob parentMob, Mob targetMob)
    {
        if (parentMob == ParentMob)
        {
            if (ParentMob.CurrentCombo)
            {
                targetMob.MobStatusEffects.AddEffect(ParentMob.CurrentCombo.elementType, Duration, ParentMob.MobData.PassiveDamage);
                ShowMessage(targetMob, ParentMob.CurrentCombo.elementType);
            }
            else
            {
                targetMob.MobStatusEffects.AddEffect(ParentMob.MobData.AttackElement, Duration, ParentMob.MobData.PassiveDamage);
                ShowMessage(targetMob, ParentMob.MobData.AttackElement);
            }
        }
    }

    private void ShowMessage(Mob targetMob, ElementType elementType)
    {
        switch (elementType)
        {
            case ElementType.Fire:
                targetMob.UI.ShowText("Подожжен!", Color.red);
                break;
            case ElementType.Air:
                targetMob.UI.ShowText("Расход стамины увеличен!", Color.cyan);
                break;
            case ElementType.Earth:
                targetMob.UI.ShowText("Опутан лозами", Color.green);
                break;
        }
    }
    
    public override void Use(Mob targetMob)
    {
        throw new System.NotImplementedException();
    }
}
