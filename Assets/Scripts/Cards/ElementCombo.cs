using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    [CreateAssetMenu(fileName = "Card Combo", menuName = "Data/Card Combo", order = 2)]
    public class ElementCombo : ScriptableObject, IComparable<ElementCombo>
    {
        public List<SpecificCondition> specificConditions = new();
        public List<MixedCondition> mixedConditions = new();
        public List<ElementCondition> elementConditions = new();
        public List<RankCondition> rankConditions = new();
        
        public ElementType damageType;
        public float damageMultiplier;
        public string comboName;
        public string description;

        [Header("Special Effects")]
        [Tooltip("Сжигает всю стамину до нуля или станит на несколько ходов")]
        public bool stun;
        public int stunTime;
        [Tooltip("Пробивает щит")]
        public bool ignoreDefense;
        [Tooltip("Дамажит всех мобов на арене")]
        public bool aoeAttack;
        
        public string GetId()
        {
            return name;
        }

        public int CompareTo(ElementCombo other)
        {
            if (other == null) return 1;
            return string.Compare(comboName, other.comboName, StringComparison.OrdinalIgnoreCase);
        }
    }
}