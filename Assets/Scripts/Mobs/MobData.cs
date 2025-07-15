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

    public enum MobType
    {
        Enemy,
        Boss,
        Ally,
        Hero
    }
    
    [CreateAssetMenu(fileName = "Mob's Data", menuName = "Data/Mob's Data", order = 1)]
    public class MobData : ScriptableObject
    {
        [Header("Mob's Data")]
        [SerializeField]
        private string mobName;
        public string MobName => mobName;
        
        [SerializeField]
        private MobType mobType;
        public MobType Type => mobType;

        public Sprite mobIcon;

        [SerializeField] private int hireCost;
        public int HireCost => hireCost;
    
        [Header("Mob's Stats")]
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
        private float attackDamage;
        public float AttackDamage => attackDamage;

        [SerializeField]
        private float attackCost;
        public float AttackCost => attackCost;

        [SerializeField]
        private int skillDamage;
        public float SkillDamage => skillDamage;

        [SerializeField]
        private float skillCost;
        public float SkillCost => skillCost;
        
        [Header("Mob Prefab")]
        [SerializeField]
        private GameObject MobPrefab;
        public GameObject mobPrefab => MobPrefab;

        public string GetId()
        {
            return name;
        }
    }
}
