using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Inventory
{
    public class ItemCard : Item
    {
        protected override void Start()
        {
            base.Start();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            Debug.Log("Item Card Clicked");
        }
        
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
        }
        
        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
        }
    }
}
