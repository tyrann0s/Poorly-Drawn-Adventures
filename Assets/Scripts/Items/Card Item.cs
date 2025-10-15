using Managers;
using TMPro;
using UI.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace Items
{
    public class CardItem : ItemPref
    {
        [SerializeField]
        private TMP_Text rankText;

        [SerializeField]
        private Image elementImage;

        private Element cardElement = new ();
        private int cardRank;

        public override void Initialize(Item item, ElementType elementType, int rank)
        {
            base.Initialize(item, elementType, rank);
            cardElement.CurrentElementType = elementType;
            cardRank = rank;
            
            rankText.text = cardRank.ToString();
            elementImage.sprite = ResourceManager.Instance.Icons.GetIcon(cardElement.CurrentElementType);
            enabled = false;
        }

        public override void Use()
        {
            throw new System.NotImplementedException();
        }
    }
}