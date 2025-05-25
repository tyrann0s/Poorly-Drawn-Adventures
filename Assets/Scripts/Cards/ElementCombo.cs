using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    [CreateAssetMenu(fileName = "Card Combo", menuName = "Data/Card Combo", order = 2)]
    public class ElementCombo : ScriptableObject
    {
        public bool useRanksInsteadOfElements;
        public List<ElementType> elements;
        [Range(1, 6)] public List<int> ranks;
        public ElementType damageType;
        public float damageMultiplier;
        public string comboName;
        public string description;
    }
}