using Mobs;
using Mobs.Status_Effects;
using UnityEngine;

public abstract class SEElemental : StatusEffect
{
    public float Damage { get; protected set; }
    
    public abstract override void ApplyEffect(Mob parentMob);
}
