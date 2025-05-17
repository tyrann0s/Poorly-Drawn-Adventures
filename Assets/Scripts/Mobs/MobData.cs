using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mob's Data", menuName = "Data/Mob's Data", order = 1)]
public class MobData : ScriptableObject
{
    [SerializeField]
    private string mobName;
    public string MobName => mobName;

    [SerializeField]
    private int maxHP;
    public int MaxHP => maxHP;

    [SerializeField]
    private int maxStamina;
    public int MaxStamina => maxStamina;

    [SerializeField]
    private int defenseCost;
    public int DefenseCost => defenseCost;

    [SerializeField]
    private int attack1Damage;
    public int Attack1Damage => attack1Damage;

    [SerializeField]
    private int attack1Cost;
    public int Attack1Cost => attack1Cost;

    [SerializeField]
    private int attack2Damage;
    public int Attack2Damage => attack2Damage;

    [SerializeField]
    private int attack2Cost;
    public int Attack2Cost => attack2Cost;
}
