using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Base.UI
{
    public class MobElement : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI nameText;

        public void SetUp(Sprite image, string name)
        {
            icon.sprite = image;
            nameText.text = name;       
        }
    }
}
