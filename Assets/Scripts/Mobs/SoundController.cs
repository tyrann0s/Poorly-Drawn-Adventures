using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField]
    private AudioSource pick, attack1, attack2, death, souls, getDamage, commonDamage, shieldDamage, move;

    public void Pick()
    {
        pick.Play();
    }

    public void Attack1()
    {
        attack1.Play();
    }

    public void Attack2()
    {
        attack2.Play();
    }

    public void Death()
    {
        commonDamage.Play();
        death.Play();
        souls.Play();
    }

    public void GetDamage()
    {
        getDamage.Play();
        commonDamage.Play();
    }

    public void GetShieldDamage()
    {
        shieldDamage.Play();
    }

    public void StartMove()
    {
        move.Play();
    }

    public void StopMove()
    {
        move.Stop();
    }
}
