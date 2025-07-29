using System;
using Mobs;
using Mobs.Skills;
using UnityEngine;

public class EvasionSkill : PassiveSkill
{
    [SerializeField] private float chance = 0.5f;
    public float Chance => chance;
    
    private void Start()
    {
        SkillName = "Evasion";
        IsAttack = false;
        IsRanged = false;
    }

    public override void Use(Mob targetMob)
    {
        Debug.Log($"{ParentMob} evade!");
    }
}
