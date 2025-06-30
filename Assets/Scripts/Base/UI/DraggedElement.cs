using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Base.UI
{
    public class DraggedElement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private Canvas canvas;
        private IDraggable originalDraggable;
        private Image image;

        private void Awake()
        {
            canvas = gameObject.AddComponent<Canvas>();
            gameObject.AddComponent<GraphicRaycaster>();
            image = GetComponent<Image>();
            originalDraggable = GetComponent<IDraggable>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            // Сохраняем исходную позицию и родителя
            originalDraggable.SetOriginalParent();
            originalDraggable.SetOriginalPosition();
            
            // Перемещаем объект на верхний слой для корректного отображения
            transform.SetParent(canvas.transform, true);
            image.raycastTarget = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            // Конвертируем экранные координаты в мировые
            Vector3 worldPosition;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
                canvas.transform as RectTransform, 
                eventData.position, 
                eventData.pressEventCamera, 
                out worldPosition))
            {
                transform.position = worldPosition;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            image.raycastTarget = true;
            transform.SetParent(originalDraggable.GetOriginalParent(), true);
            transform.position = originalDraggable.GetOriginalPosition();
        }
    }
}