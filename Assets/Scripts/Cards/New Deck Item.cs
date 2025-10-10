using System;
using System.Collections;
using DG.Tweening;
using Managers;
using TMPro;
using UI.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cards
{
    public class NewDeckItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject text;
        
        private CurvedLayoutGroup layoutGroup;
        private Vector3 originalPosition;
        private bool isActive = false;
        private Item parentItem;

        public void Initialize(Item item)
        {
            layoutGroup = GetComponentInChildren<CurvedLayoutGroup>();
            originalPosition = transform.position;
            StartCoroutine(OpenDeck());
            parentItem = item;
        }

        private void CreateFloatingEffect()
        {
            // Вертикальное парение (вверх-вниз)
            transform.DOMoveY(originalPosition.y + 5f, 1)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);

            // Легкое покачивание по горизонтали
            transform.DOMoveX(originalPosition.x + 3f, 1)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo)
                .SetDelay(0.3f);
        }

        private IEnumerator OpenDeck()
        {
            float startRadius = layoutGroup.radius;
            float targetRadius = 150f;
            
            DOTween.To(
                getter: () => layoutGroup.radius,
                setter: (value) => {
                    layoutGroup.radius = value;
                    layoutGroup.CalculateLayoutInputHorizontal();
                    layoutGroup.CalculateLayoutInputVertical();
                    layoutGroup.SetLayoutHorizontal();
                    layoutGroup.SetLayoutVertical();
                },
                endValue: targetRadius,
                duration: .5f
            )
            .SetEase(Ease.InOutCirc)
            .SetLoops(2, LoopType.Yoyo); // 2 цикла = туда и обратно
            
            yield return new WaitForSeconds(1f); // время на полную анимацию
            isActive = true;
            text.SetActive(true);
            CreateFloatingEffect();
        }

        private void OnDestroy()
        {
            // Останавливаем все твины при уничтожении объекта
            transform.DOKill();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (isActive)
            {
                //ServiceLocator.Get<UIManager>().ExitChangeCards();
                ServiceLocator.Get<CardPanel>().ResetDeck();
                parentItem.UseComplete();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (isActive) transform.localScale = Vector3.one * 1.2f;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (isActive) transform.localScale = Vector3.one;
        }
    }
}