using System;
using Mobs.Status_Effects;
using UnityEngine;
using UnityEngine.UI;

namespace Mobs.UI
{
    public class StatusEffectsPanel : MonoBehaviour
    {
        [SerializeField] private Image defenseImage, stunImage, fireImage, airImage, earthImage;

        public void ShowEffect(StatusEffect effect)
        {
            switch (effect.EffectType)
            {
                case StatusEffectType.Defense:
                    defenseImage.gameObject.SetActive(true);
                    break;
                case StatusEffectType.Stun:
                    stunImage.gameObject.SetActive(true);
                    break;
                case StatusEffectType.Elemental:
                    switch (effect.GetType())
                    {
                        case { } type when type == typeof(SEFire):
                            fireImage.gameObject.SetActive(true);
                            break;
                        case { } type when type == typeof(SEAir):
                            airImage.gameObject.SetActive(true);
                            break;
                        
                        case { } type when type == typeof(SEEarth):
                            earthImage.gameObject.SetActive(true);
                            break;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public void HideEffect(StatusEffect effect)
        {
            switch (effect.EffectType)
            {
                case StatusEffectType.Defense:
                    defenseImage.gameObject.SetActive(false);
                    break;
                case StatusEffectType.Stun:
                    stunImage.gameObject.SetActive(false);
                    break;
                case StatusEffectType.Elemental:
                    switch (effect.GetType())
                    {
                        case { } type when type == typeof(SEFire):
                            fireImage.gameObject.SetActive(false);
                            break;
                        case { } type when type == typeof(SEAir):
                            airImage.gameObject.SetActive(false);
                            break;
                        
                        case { } type when type == typeof(SEEarth):
                            earthImage.gameObject.SetActive(false);
                            break;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
