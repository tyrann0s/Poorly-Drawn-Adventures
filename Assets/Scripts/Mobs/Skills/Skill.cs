using System;
using Mobs;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Skill : ScriptableObject
{
    protected Mob ParentMob { get; private set; }
    
    [SerializeField] private string skillName;
    public string SkillName
    {
        get => skillName;
        set => skillName = value;
    }

    [SerializeField] private float amount = 50f;
    public float Amount => amount;

    [SerializeField] private float cost = 50f;
    public float Cost => cost;

    [SerializeField] private bool isAttack;
    public bool IsAttack
    {
        get => isAttack;
        set => isAttack = value;
    }

    [SerializeField] private bool isRanged;
    public bool IsRanged
    {
        get => isRanged;
        set => isRanged = value;
    }

    public void Initialize(Mob parentMob)
    {
        ParentMob = parentMob;
    }

    public abstract void Use(Mob targetMob);
}
