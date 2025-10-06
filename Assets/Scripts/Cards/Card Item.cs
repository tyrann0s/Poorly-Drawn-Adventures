using System;
using DG.Tweening;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Cards
{
    public class CardItem : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text rankText;

        [SerializeField]
        private Image elementImage;

        private Element cardElement = new ();
        private int cardRank;
        private Vector3 originalPosition;

        public void InitializeCard(ElementType elementType, int rank)
        {
            cardElement.CurrentElementType = elementType;
            cardRank = rank;
            
            rankText.text = cardRank.ToString();
            elementImage.sprite = ResourceManager.Instance.Icons.GetIcon(cardElement.CurrentElementType);
            enabled = false;
            
            // Сохраняем изначальную позицию и поворот
            originalPosition = transform.position;
            
            // Создаем магический эффект парения
            CreateFloatingEffect();
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