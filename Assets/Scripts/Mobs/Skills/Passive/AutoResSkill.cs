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
        
        MobManager.OnMobDied += Use;
        GameManager.OnNewWave += ResetReviveCount;
    }
    
    private void OnDisable()
    {
        MobManager.OnMobDied -= Use;   
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
    }
    
    public override void Use(Mob targetMob)
    {
        base.Use(targetMob);
        
        if (targetMob.IsHostile == ParentMob.IsHostile)
        {
            if (currentReviveCount > 0)
            {
                targetMob.MobCombatSystem.Revive();
                currentReviveCount--;
            }
        }
    }
}
