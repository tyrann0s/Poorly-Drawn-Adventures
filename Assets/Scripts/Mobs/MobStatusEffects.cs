using System;
using System.Collections.Generic;
using System.Linq;
using Mobs.Status_Effects;

namespace Mobs
{
    public class MobStatusEffects : MobComponent
    {
        public List<StatusEffect> StatusEffects { get; private set; } = new();

        public void AddEffect(Mob parentMob, StatusEffectType effectType, int duration)
        {
            if (duration <= 0)
                throw new ArgumentException("Длительность эффекта должна быть положительным числом", nameof(duration));

            var existingEffect = StatusEffects.FirstOrDefault(effect => effect.EffectType == effectType);

            if (existingEffect != null)
            {
                UpdateExistingEffect(existingEffect, duration);
            }
            else
            {
                CreateNewEffect(parentMob, effectType, duration);
            }
        }

        private void UpdateExistingEffect(StatusEffect effect, int duration)
        {
            effect.Duration = duration;
            effect.ApplyEffect();
        }

        private void CreateNewEffect(Mob parentMob, StatusEffectType effectType, int duration)
        {
            var newEffect = StatusEffect.Create(parentMob, effectType, duration);
            if (newEffect == null)
                throw new InvalidOperationException($"Не удалось создать эффект типа {effectType}");

            StatusEffects.Add(newEffect);
            newEffect.ApplyEffect();
        }

        public void UpdateEffectsDuration()
        {
            foreach (var effect in StatusEffects)
            {
                effect.Duration--;
            }
            
            foreach (var effect in StatusEffects.ToList())
            {
                if (effect.Duration <= 0)
                {
                    StatusEffects.Remove(effect);
                }
            }
        }

        public bool CheckShield()
        {
            return StatusEffects.Any(effect => effect.EffectType == StatusEffectType.Defense);
        }

        public bool CheckStun()
        {
            return StatusEffects.Any(effect => effect.EffectType == StatusEffectType.Stun);
        }
    }
}
