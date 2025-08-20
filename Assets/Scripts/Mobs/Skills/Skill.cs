using UnityEngine;

namespace Mobs.Skills
{
    [System.Serializable]
    public abstract class Skill
    {
        public string SkillName { get; protected set; }
        public bool IsAttack { get; protected set; }
        public bool IsRanged { get; protected set; }
    
        public Mob ParentMob { get; private set; }
        public float Amount { get; private set; }
        public float Cost { get; private set; }
        
        public int Duration { get; protected set; }
        
        public string Description { get; protected set; }

        public virtual void Initialize(Mob parentMob, float amount, float cost, int duration)
        {
            ParentMob = parentMob;
            Amount = amount;
            Cost = cost;
        }

        public abstract void Use(Mob targetMob);
    }
}
