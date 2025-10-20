using Cards;
using DG.Tweening;
using Managers;
using UI.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Items
{
    public abstract class ItemPref : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private Vector3 originalPosition;
        protected Item parentItem;
        
        private void Awake()
        {
            // Сохраняем изначальную позицию и поворот
            originalPosition = transform.position;
            
            // Создаем магический эффект парения
            CreateFloatingEffect();
        }
        
        public virtual void Initialize(Item item)
        {
            parentItem = item;
        }

        public virtual void Initialize(Item item, ElementType elementType, int rank)
        {
            parentItem = item;
        }

        public virtual void Initialize(Item item, Sprite icon, ItemScroll scroll)
        {
            parentItem = item;
        }

        private void CreateFloatingEffect()
        {
            // Вертикальное парение (вверх-вниз)
            transform.DOMoveY(originalPosition.y + 7f, 1)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);

            // Легкое покачивание по горизонтали
            transform.DOMoveX(originalPosition.x + 5f, 1)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo)
                .SetDelay(0.3f);
        }

        private void OnDestroy()
        {
            // Останавливаем все твины при уничтожении объекта
            transform.DOKill();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Use();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.localScale = Vector3.one * 1.2f;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.localScale = Vector3.one;
        }
        
        public virtual void Use()
        {
            parentItem.UseComplete();
            ServiceLocator.Get<TargetManager>().ClearContext();
        }
    }
}
