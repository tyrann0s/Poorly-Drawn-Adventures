using System;
using Managers;
using Mobs;
using Mobs.Skills;
using UnityEngine;

public class AutoResSkill : PassiveSkill
{
    [SerializeField] private int reviveCount = 1;
    private int currentReviveCount;

    public override void Initialize(Mob parentMob, float amount, float cost, int duration = 0)
    {
        base.Initialize(parentMob, amount, cost, 1);
        
        SkillName = "Auto Revive";
        IsAttack = false;
        IsRanged = true;
        
        ResetReviveCount();
        
        MobManager.OnMobDied += Prepare;
        GameManager.OnNewWave += ResetReviveCount;
    }

    protected override void Cleanup()
    {
        MobManager.OnMobDied -= Prepare;   
        GameManager.OnNewWave -= ResetReviveCount;
        base.Cleanup();
    }

    private void ResetReviveCount()
    {
        currentReviveCount = reviveCount;
        if (ParentMob) ParentMob.PassiveAction = null;
    }

    private void Prepare(Mob targetMob)
    {
        if (ParentMob)
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
    }
    
    public override void Use(Mob targetMob)
    {
        targetMob.MobCombatSystem.Revive();
        currentReviveCount--;
    }
}
