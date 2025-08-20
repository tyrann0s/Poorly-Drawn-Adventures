using Managers;

namespace Mobs.Skills.Active
{
    [System.Serializable]
    public class AOEDamageSkill : ActiveSkill
    {
        public override void Initialize(Mob parentMob, float amount, float cost, int duration = 0)
        {
            base.Initialize(parentMob, amount, cost, 1);
            
            SkillName = "AOE Damage";
            Description = "Damages all enemies in the area.";
            IsAttack = true;
            IsRanged = true;
        }
    
        public override void Use(Mob targetMob)
        {
            foreach (var enemyMob in ServiceLocator.Get<MobManager>().EnemyMobs)
            {
                enemyMob.MobCombatSystem.GetDamage(Amount, enemyMob.CurrentCombo);
            }
        }
    }
}
