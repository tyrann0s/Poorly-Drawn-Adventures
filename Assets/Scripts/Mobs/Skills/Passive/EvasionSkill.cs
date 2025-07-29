using System;
using Mobs;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu (fileName = "Evasion", menuName = "Data/Skills/Passive Skills/Evasion", order = 1)]
public class EvasionSkill : PassiveSkill
{
    [SerializeField] private float chance = 0.5f;
    public float Chance => chance;
    
    private void Reset()
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
