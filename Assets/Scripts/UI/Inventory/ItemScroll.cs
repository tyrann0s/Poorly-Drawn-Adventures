using Items;
using Mobs.Skills;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Inventory
{
    public abstract class ItemScroll : Item
    {
        [SerializeField] private GameObject itemScrollPrefab;
        
        protected override void Start()
        {
            base.Start();
        }
        
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            GameObject go = Instantiate(itemScrollPrefab, inventory.ItemPosition);
            go.GetComponent<ScrollItem>().Initialize(IcSprite, this);
        }
        
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
        }
        
        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
        }

        public abstract void Use();
    }
}
