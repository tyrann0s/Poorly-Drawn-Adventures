using System.Collections;
using Cards;
using DG.Tweening;
using UI.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Items
{
    public class NewDeckItem : ItemPref, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private Item parentItem;

        public override void Initialize(Item item)
        {
            parentItem = item;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //ServiceLocator.Get<UIManager>().ExitChangeCards();
            ServiceLocator.Get<CardPanel>().ResetDeck();
            parentItem.UseComplete();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.localScale = Vector3.one * 1.2f;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.localScale = Vector3.one;
        }
    }
}