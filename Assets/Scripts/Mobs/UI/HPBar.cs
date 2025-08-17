using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mobs
{
    public class HPBar : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI hpText, staminaText;
        
        private float maxHP, maxStamina; 

        public void Init(float hp, float stamina)
        {
            maxHP = hp;
            maxStamina = stamina;
            
            hpText.text = $"{maxHP}/{maxHP}";
            staminaText.text = $"{maxStamina}/{maxStamina}";
        }

        public void UpdateHP(float value)
        {
            hpText.text = $"{value}/{maxHP}";
        }
        
        public void UpdateStamina(float value)
        {
            staminaText.text = $"{value}/{maxStamina}";
        }
    }
}
