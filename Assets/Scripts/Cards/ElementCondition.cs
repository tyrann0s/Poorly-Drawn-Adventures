using UnityEngine;

namespace Cards
{
    [System.Serializable]
    public class SpecificCondition
    {
        public ElementType elementType;
        [Range(1, 6)]
        public int rank;
    }

    [System.Serializable]
    public class MixedCondition
    {
        public int amount;
    }

    [System.Serializable]
    public class ElementCondition
    {
        public ElementType elementType;
    }

    [System.Serializable]
    public class RankCondition
    {
        [Range(1, 6)]
        public int amount;
    }
}