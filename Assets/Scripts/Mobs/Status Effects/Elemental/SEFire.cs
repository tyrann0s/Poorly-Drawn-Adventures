using Mobs;
using UnityEngine;

public class SEFire : SEElemental
{
    private int skipTurn = 1;

    public SEFire(float damage)
    {
        IsNegative = true;
        IsActive = true;
        Damage = damage;
    }
    
    public override void ApplyEffect(Mob parentMob)
    {
        if (skipTurn > 0)
        {
            skipTurn = 0;
            return;
        }
        parentMob.MobCombatSystem.GetDamage(Damage, null);
    }
}
