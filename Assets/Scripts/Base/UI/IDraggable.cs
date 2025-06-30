using UnityEngine;

namespace Base.UI
{
    public interface IDraggable
    {
        public Transform GetOriginalParent();
        public Vector3 GetOriginalPosition();
        public void SetOriginalParent();
        public void SetOriginalPosition();
        
        public void ApplyDrag(IDraggable draggedElement);
    }
}