using Mobs;
using UnityEngine;

public class SEFire : SEElemental
{
    public SEFire()
    {
        IsNegative = true;
        IsActive = true;
    }
    
    public override void ApplyEffect(Mob parentMob)
    {
        Debug.Log("Наносим урон огнем");
    }
}
