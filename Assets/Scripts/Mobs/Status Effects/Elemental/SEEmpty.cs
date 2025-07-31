using Mobs;
using UnityEngine;

public class SEEmpty : SEElemental
{
    public override void ApplyEffect(Mob parentMob)
    {
        Debug.Log($"ПУСТОЙ ЕВЕНТ! У {parentMob} неверно выставлен элемент атаки");
    }
}
