using TMPro;
using UnityEngine;

namespace Base.UI
{
    public class CoinsPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI coinsText;

        public void UpdateText(string text)
        {
            coinsText.text = text;
        }
    }
}