using UnityEngine;

namespace Mobs
{
    public enum AttackType
    {
        Melee,
        Ranged,
        UnStun,
        CastShield
    }
    
    [CreateAssetMenu(fileName = "Mob's Data", menuName = "Data/Mob's Data", order = 1)]
    public class MobData : ScriptableObject
    {
        [SerializeField]
        private string mobName;
        public string MobName => mobName;
    
        [SerializeField]
        private ElementType vulnerableTo;
        public ElementType VulnerableTo => vulnerableTo;
    
        [SerializeField]
        private ElementType immuneTo;
        public ElementType ImmuneTo => immuneTo;

        [SerializeField]
        private float maxHP;
        public float MaxHP => maxHP;

        [SerializeField]
        private int maxStamina;
        public float MaxStamina => maxStamina;

        [SerializeField]
        private int defenseCost;
        public float DefenseCost => defenseCost;

        [SerializeField] private AttackType attackType;
        public AttackType AttackType => attackType;

        [SerializeField] private int maxTargets = 1;
        public int MaxTargets => maxTargets;

        [SerializeField]
        private float attack1Damage;
        public float Attack1Damage => attack1Damage;

        [SerializeField]
        private float attack1Cost;
        public float Attack1Cost => attack1Cost;

        [SerializeField]
        private int attack2Damage;
        public float Attack2Damage => attack2Damage;

        [SerializeField]
        private float attack2Cost;
        public float Attack2Cost => attack2Cost;
    }
}
