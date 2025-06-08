using System;

namespace Mobs.Status_Effects
{
    public enum StatusEffectType
    {
        Defense,
        Stun
    }
    public class StatusEffect
    {
        public StatusEffectType EffectType { get; set; }
        public int Duration { get; set; }

        public StatusEffect Create(StatusEffectType effectType, int duration)
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
    }
}
