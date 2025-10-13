using DG.Tweening;
using Mobs.Skills;
using UI.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace Items
{
    public abstract class ItemPref : MonoBehaviour
    {
        private Vector3 originalPosition;

        private void Awake()
        {
            // Сохраняем изначальную позицию и поворот
            originalPosition = transform.position;
            
            // Создаем магический эффект парения
            CreateFloatingEffect();
        }
        
        public virtual void Initialize(Item item)
        {
        }

        public virtual void Initialize(ElementType elementType, int rank)
        {
        }

        public virtual void Initialize(Sprite icon, ItemScroll scroll)
        {
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
    }
}
