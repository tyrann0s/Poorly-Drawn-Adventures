using Cards;
using Items;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Inventory
{
    public class ItemDeck : Item
    {
        [SerializeField] private GameObject changeDeckPrefab;
        
        protected override void Start()
        {
            base.Start();
        }
        
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            ServiceLocator.Get<UIManager>().EnterChangeCards();
            GameObject go = Instantiate(changeDeckPrefab, inventory.ItemPosition);
            go.GetComponent<NewDeckItem>().Initialize(this);
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
