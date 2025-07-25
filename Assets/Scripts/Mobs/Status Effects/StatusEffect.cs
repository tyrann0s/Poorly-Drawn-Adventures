using System;

namespace Mobs.Status_Effects
{
    public enum StatusEffectType
    {
        Defense,
        Stun
    }
    public abstract class StatusEffect
    {
        public StatusEffectType EffectType { get; set; }
        public int Duration { get; set; }
        public Mob ParentMob { get; set; }
        public bool IsNegative { get; set; }

        public static StatusEffect Create(Mob parentMob, StatusEffectType effectType, int duration)
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
            
            effect.ParentMob = parentMob;
            effect.EffectType = effectType;
            effect.Duration = duration;
            return effect;
        }
        
        public abstract void ApplyEffect();
    }
}
