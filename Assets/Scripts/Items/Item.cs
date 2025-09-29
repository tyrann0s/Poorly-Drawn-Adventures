using Mobs;
using UnityEngine;

namespace Items
{
    public abstract class Item : MonoBehaviour
    {
        [SerializeField]
        private string itemName;
        public string ItemName
        {
            get => itemName;
            protected set => itemName = value;
        }

        [SerializeField]
        private string description;
        public string Description => description;
        
        [SerializeField]
        private Sprite icon;
        public Sprite Icon => icon;
        
        public abstract void Use();

        // Инициализация карт
        public virtual void Initialize(ElementType elementType, int rank)
        {
        }
        
        // Инициализация колоды
        public virtual void Initialize()
        {
        }
        
        // Инициализация атакующего свитка
        public virtual void Initialize(ElementType damageType, bool isAOE, Mob target)
        {
        }
        
        // Инициализация свитка поддержки
        public virtual void Initialize(bool isAOE, Mob target)
        {
        }
    }
}
