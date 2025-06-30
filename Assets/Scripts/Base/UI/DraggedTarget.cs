using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Base.UI
{
    public class DraggedTarget:MonoBehaviour, IDropHandler
    {
        private IDraggable parentDraggable;
        private void Awake()
        {
            parentDraggable = GetComponent<IDraggable>();
        }
        
        public void OnDrop(PointerEventData eventData)
        {
            // Получаем перетаскиваемый объект
            IDraggable draggedElement = eventData.pointerDrag.GetComponent<MobElement>();
            parentDraggable.ApplyDrag(draggedElement);
        }
    }
}