using UnityEngine;
using UnityEngine.Serialization;

namespace Mobs
{
    public enum AttackType
    {
        Melee,
        Ranged,
        Heal,
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

        [FormerlySerializedAs("attack1Damage")] [SerializeField]
        private float attackDamage;
        public float AttackDamage => attackDamage;

        [FormerlySerializedAs("attack1Cost")] [SerializeField]
        private float attackCost;
        public float AttackCost => attackCost;

        [FormerlySerializedAs("attack2Damage")] [SerializeField]
        private int skillDamage;
        public float SkillDamage => skillDamage;

        [FormerlySerializedAs("attack2Cost")] [SerializeField]
        private float skillCost;
        public float SkillCost => skillCost;
    }
}
