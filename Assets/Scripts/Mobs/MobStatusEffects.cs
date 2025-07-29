using System;
using System.Collections.Generic;
using System.Linq;
using Mobs.Status_Effects;

namespace Mobs
{
    public class MobStatusEffects : MobComponent
    {
        public List<StatusEffect> StatusEffects { get; private set; } = new();

        public void AddEffect(StatusEffectType effectType, int duration)
        {
            if (duration <= 0)
                throw new ArgumentException("Длительность эффекта должна быть положительным числом", nameof(duration));
            
            var newEffect = StatusEffect.Create(effectType, duration);
            if (newEffect == null)
                throw new InvalidOperationException($"Не удалось создать эффект типа {effectType}");

            CreateOrUpdateEffect(newEffect, duration);
        }
        
        public void AddEffect(ElementType elementType, int duration)
        {
            if (duration <= 0)
                throw new ArgumentException("Длительность эффекта должна быть положительным числом", nameof(duration));
            
            var newEffect = StatusEffect.Create(elementType, duration);

            CreateOrUpdateEffect(newEffect, duration);
        }

        private void CreateOrUpdateEffect(StatusEffect effect, int duration)
        {
            if (effect.IsNegative && ParentMob.MobData.PassiveSkill is StatusEffectImmuneSkill) return;
            
            if (StatusEffects.Contains(effect))
            {
                UpdateExistingEffect(effect, duration);
            }
            else
            {
                StatusEffects.Add(effect);
                effect.ApplyEffect(ParentMob);
            }
        }

        private void UpdateExistingEffect(StatusEffect effect, int duration)
        {
            effect.Duration = duration;
            effect.ApplyEffect(ParentMob);
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

        public void UseActiveEffects()
        {
            foreach (var statusEffect in StatusEffects)
            {
                statusEffect.ApplyEffect(ParentMob);
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
