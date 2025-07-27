using Managers;
using Mobs;
using UnityEngine;

[ CreateAssetMenu (fileName = "AOE Damage", menuName = "Data/Skills/Active Skills/AOE Damage", order = 1)]
public class AOEDamageSkill : ActiveSkill
{
    private void Reset()
    {
        SkillName = "AOE Damage";
        
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
