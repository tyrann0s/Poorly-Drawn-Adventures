using System;
using Mobs;
using UnityEngine;

public abstract class ActiveSkill : Skill
{
    public abstract override void Use(Mob targetMob);
}
