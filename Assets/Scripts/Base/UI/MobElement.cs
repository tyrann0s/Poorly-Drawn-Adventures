using Mobs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Base.UI
{
    public class MobElement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
    {
        [SerializeField] private bool isTarget;
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI nameText;
        
        private MobData mobData;
        private Transform originalParent;
        private Vector3 originalPosition;
        private Canvas canvas;

        private void Awake()
        {
            canvas = GetComponentInParent<Canvas>();
        }

        public void SetUp(MobData mobData)
        {
            this.mobData = mobData;
            icon.sprite = mobData.mobIcon;
            nameText.text = mobData.MobName;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (isTarget) return;
            
            // Сохраняем исходную позицию и родителя
            originalParent = transform.parent;
            originalPosition = transform.position;
            
            // Перемещаем объект на верхний слой для корректного отображения
            transform.SetParent(canvas.transform, true);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (isTarget) return;
            // Перемещаем объект за курсором
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (isTarget) return;
            
            // Если объект не был сброшен на подходящую цель, возвращаем на место
            if (transform.parent == canvas.transform)
            {
                transform.SetParent(originalParent, true);
                transform.position = originalPosition;
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            // Получаем перетаскиваемый объект
            MobElement draggedElement = eventData.pointerDrag.GetComponent<MobElement>();
            
            if (draggedElement && draggedElement != this && isTarget)
            {
                // Копируем данные из перетаскиваемого объекта в текущий
                CopyDataFrom(draggedElement);
                
                // Возвращаем перетаскиваемый объект на исходную позицию
                draggedElement.transform.SetParent(draggedElement.originalParent, true);
                draggedElement.transform.position = draggedElement.originalPosition;
            }
        }

        private void CopyDataFrom(MobElement source)
        {
            if (source.mobData)
            {
                SetUp(source.mobData);
                Debug.Log($"Скопированы данные: {source.mobData.MobName} в {gameObject.name}");
            }
        }
    }
}