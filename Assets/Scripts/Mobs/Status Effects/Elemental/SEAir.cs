using Mobs;
using UnityEngine;

public class SEAir : SEElemental
{
    public SEAir()
    {
        IsNegative = true;
    }
    
    public override void ApplyEffect(Mob parentMob)
    {
        Debug.Log("Увеличивает расход стамины");
    }
}
