using Cards;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Inventory
{
    public class ItemCard : Item
    {
        [SerializeField] private GameObject cardPrefab;
        
        public ElementType CardElement { get; private set; }
        public int CardRank { get; private set; }
        
        protected override void Start()
        {
            base.Start();
            SetUpCard(ElementType.Fire, 6);
        }

        public void SetUpCard(ElementType element, int rank)
        {
            CardElement = element;
            CardRank = rank;
            SetDescription($"New card: {CardElement} {CardRank}");
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            ServiceLocator.Get<UIManager>().EnterItemCardChange(this);
            GameObject go = Instantiate(cardPrefab, inventory.ItemPosition);
            CardItem card = go.GetComponent<CardItem>();
            card.InitializeCard(CardElement, CardRank);
        }
        
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
        }
        
        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
        }

        protected override void Cancel()
        {
            base.Cancel();
            ServiceLocator.Get<UIManager>().ExitChangeCards();
        }
    }
}
