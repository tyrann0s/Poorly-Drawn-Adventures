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

    var existingEffect = StatusEffects.FirstOrDefault(effect => effect.EffectType == effectType);
    
    if (existingEffect != null)
    {
        UpdateExistingEffect(existingEffect, duration);
    }
    else
    {
        CreateNewEffect(effectType, duration);
    }
}

private void UpdateExistingEffect(StatusEffect effect, int duration)
{
    effect.Duration = duration;
}

private void CreateNewEffect(StatusEffectType effectType, int duration)
{
    var newEffect = new StatusEffect().Create(effectType, duration);
    if (newEffect == null)
        throw new InvalidOperationException($"Не удалось создать эффект типа {effectType}");
        
    StatusEffects.Add(newEffect);
}
        
        public bool CheckShield()
        {
            return false;
        }

        public bool CheckStun()
        {
            return false;
        }
    }
}
