using System;
using Managers;
using Mobs;
using UnityEngine;

[CreateAssetMenu (fileName = "Auto Revive", menuName = "Data/Skills/Passive Skills/Auto Revive", order = 1)]
public class AutoResSkill : PassiveSkill
{
    [SerializeField] private int reviveCount = 1;
    private int currentReviveCount;

    private void OnEnable()
    {
        ResetReviveCount();
        
        MobManager.OnMobDied += Prepare;
        GameManager.OnNewWave += ResetReviveCount;
    }
    
    private void OnDisable()
    {
        MobManager.OnMobDied -= Prepare;   
        GameManager.OnNewWave -= ResetReviveCount;
    }

    private void Reset()
    {
        SkillName = "Auto Revive";
        IsAttack = false;
        IsRanged = true;
    }

    private void ResetReviveCount()
    {
        currentReviveCount = reviveCount;
        ParentMob.PassiveAction = null;
    }

    private void Prepare(Mob targetMob)
    {
        if (targetMob.IsHostile == ParentMob.IsHostile)
        {
            if (currentReviveCount > 0)
            {
                ParentMob.PassiveAction = new MobAction(ActionType.PassiveSkill, ParentMob, targetMob);
                ServiceLocator.Get<QueueManager>().InjectAction(ParentMob.PassiveAction);
            }
        }
    }
    
    public override void Use(Mob targetMob)
    {
        targetMob.MobCombatSystem.Revive();
        currentReviveCount--;
    }
}
