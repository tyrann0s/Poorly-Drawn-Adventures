using System;
using UnityEngine;

namespace Mobs.Status_Effects
{
    public enum StatusEffectType
    {
        Defense,
        Stun,
        Elemental
    }
    public abstract class StatusEffect
    {
        public StatusEffectType EffectType { get; protected set; }
        public int Duration { get; set; }
        public bool IsNegative { get; protected set; }
        public bool IsActive { get; protected set; }

        public static StatusEffect Create(StatusEffectType effectType, int duration)
        {
            StatusEffect effect;
            switch (effectType)
            {
                case StatusEffectType.Defense:
                    effect = new SEDefense();
                    break;
                case StatusEffectType.Stun:
                    effect = new SEStun();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(effectType), effectType, null);
            }
            
            effect.EffectType = effectType;
            effect.Duration = duration;
            return effect;
        }
        
        public static StatusEffect Create(ElementType elementType, int duration)
        {
            StatusEffect effect;
            switch (elementType)
            {
                case ElementType.Fire:
                    effect = new SEFire();
                    break;
                case ElementType.Air:
                    effect = new SEAir();
                    break;
                case ElementType.Earth:
                    effect = new SEEarth();
                    break;
                default:
                    effect = new SEEmpty();
                    break;
            }
            
            effect.EffectType = StatusEffectType.Elemental;
            effect.Duration = duration;
            return effect;
        }
        
        public abstract void ApplyEffect(Mob parentMob);
    }
}
