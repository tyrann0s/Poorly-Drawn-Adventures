using TMPro;
using UI.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace Items
{
    public class ScrollItem : ItemPref
    {
        private Image iconImage;

        public override void Initialize(Item item, Sprite icon, ItemScroll scroll)
        {
            base.Initialize(item, icon, scroll);
            iconImage = GetComponent<Image>();
            iconImage.sprite = icon;

            TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();
            text.text = item.Description;
        }
    }
}
