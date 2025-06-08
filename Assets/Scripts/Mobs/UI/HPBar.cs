using UnityEngine;
using UnityEngine.UI;

namespace Mobs
{
    public class HPBar : MonoBehaviour
    {
        private Slider slider;
        public int MaxValue { get; set; }

        private void Awake()
        {
            slider = GetComponentInChildren<Slider>();
        }

        public void Init(float maxValue)
        {
            slider.maxValue = maxValue;
            slider.value = maxValue;
        }

        public void UpdateHP(float value)
        {
            if (slider != null)
            {
                slider.value = value;
                var text = slider.GetComponentInChildren<Text>();
                if (text != null)
                {
                    text.text = $"{value}/{slider.maxValue}";
                }
            }
        }

        public void UpdateMaxHP(int value)
        {
            if (slider != null)
            {
                slider.maxValue = value;
                var text = slider.GetComponentInChildren<Text>();
                if (text != null)
                {
                    text.text = $"{slider.value}/{value}";
                }
            }
        }
    }
}
