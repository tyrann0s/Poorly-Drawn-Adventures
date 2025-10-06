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
        protected global::Inventory inventory;

        protected virtual void Start()
        {
            inventory = GetComponentInParent<global::Inventory>();
            icon = GetComponent<Image>();
            icon.sprite = iconSprite;
        }

        protected void ShowIcon(bool show)
        {
            icon.enabled = show;
        }

        public void ActivateItem()
        {
            ShowIcon(true);
            Cancel();
        }

        protected virtual void Cancel()
        {
            
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            ShowIcon(false);
            inventory.ResetItem(this);
        }

        public void SetDescription(string text)
        {
            description = text;
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            inventory.ShowDescription(description);
            transform.localScale = Vector3.one * 1.3f;
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            inventory.HideDescription();
            transform.localScale = Vector3.one;
        }

        public void UseComplete()
        {
            inventory.RemoveItem(this);
        }
    }
}
