using Mobs;
using UnityEngine;

public class SEAir : SEElemental
{
    public SEAir(float increasedCost)
    {
        IsNegative = true;
        Damage = increasedCost;
    }
    
    public override void ApplyEffect(Mob parentMob)
    {
        parentMob.UI.ShowText("УВЕЛИЧЕН РАСХОД СТАМИНЫ!", Color.cyan);
    }
}
