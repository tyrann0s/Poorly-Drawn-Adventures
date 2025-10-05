using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Inventory
{
    public abstract class Item : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private string description;
        [SerializeField] private Sprite iconSprite;
    
        private Image icon;
        private global::Inventory inventory;

        protected virtual void Start()
        {
            inventory = GetComponentInParent<global::Inventory>();
            icon = GetComponent<Image>();
            icon.sprite = iconSprite;
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            inventory.ShowDescription(description);
            transform.localScale = Vector3.one * 1.1f;
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            inventory.HideDescription();
            transform.localScale = Vector3.one;
        }
    }
}
