using Mobs.Skills;
using UnityEngine;

namespace Mobs
{
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
        
        [Tooltip("Если включено, то моб будет невосприимчив к любому урону, кроме VulnerableTo")]
        [SerializeField]
        private bool totalImmune;
        public bool TotalImmune => totalImmune;

        [SerializeField]
        private float maxHP;
        public float MaxHP => maxHP;

        [SerializeField]
        private int maxStamina;
        public float MaxStamina => maxStamina;

        [SerializeField]
        private int defenseCost;
        public float DefenseCost => defenseCost;
        
        [SerializeField]
        private float attackDamage;
        public float AttackDamage => attackDamage;

        [SerializeField]
        private float attackCost;
        public float AttackCost => attackCost;

        [Header("Active Skill")]
        [SerializeReference] private ActiveSkill activeSkill;
        public ActiveSkill ActiveSkill => activeSkill;
        
        [SerializeField]
        private float skillDamage = 50;
        public float SkillDamage => skillDamage;

        [SerializeField]
        private float skillCost = 50;
        public float SkillCost => skillCost;

        [SerializeField] private int maxTargets = 1;
        public int MaxTargets => maxTargets;
        
        [SerializeField] int activeDuration = 1;
        public int ActiveDuration => activeDuration;
        
        [Header("Passive Skill")]
        [SerializeReference] private PassiveSkill passiveSkill;
        public PassiveSkill PassiveSkill => passiveSkill;
        
        [SerializeField]
        private float passiveDamage = 50;
        public float PassiveDamage => passiveDamage;
        [SerializeField] int passiveDuration = 1;
        public int PassiveDuration => passiveDuration;

        [Header("Attack Element")]
        [SerializeField] private ElementType attackElement;
        public ElementType AttackElement => attackElement;
        
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
