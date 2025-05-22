using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField]
    private AudioSource pick, attack1, attack2, death, souls, getDamage, commonDamage, shieldDamage, move;

    public void Pick()
    {
        if (pick == null)
        {
            Debug.LogWarning($"Pick sound not found on {transform.name}");
            return;
        }

        if (!pick.isPlaying)
        {
            pick.Play();
        }
    }

    public void Attack1()
    {
        if (attack1 == null)
        {
            Debug.LogWarning($"Attack1 sound not found on {transform.name}");
            return;
        }

        if (!attack1.isPlaying)
        {
            attack1.Play();
        }
    }

    public void Attack2()
    {
        if (attack2 == null)
        {
            Debug.LogWarning($"Attack2 sound not found on {transform.name}");
            return;
        }

        if (!attack2.isPlaying)
        {
            attack2.Play();
        }
    }

    public void PlayShieldDamageSound()
    {
        if (shieldDamage == null)
        {
            Debug.LogWarning($"ShieldDamage sound not found on {transform.name}");
            return;
        }

        if (!shieldDamage.isPlaying)
        {
            shieldDamage.Play();
        }
    }

    public void Death()
    {
        if (commonDamage == null)
        {
            Debug.LogWarning($"CommonDamage sound not found on {transform.name}");
        }
        else if (!commonDamage.isPlaying)
        {
            commonDamage.Play();
        }

        if (death == null)
        {
            Debug.LogWarning($"Death sound not found on {transform.name}");
        }
        else if (!death.isPlaying)
        {
            death.Play();
        }

        if (souls == null)
        {
            Debug.LogWarning($"Souls sound not found on {transform.name}");
        }
        else if (!souls.isPlaying)
        {
            souls.Play();
        }
    }

    public void GetDamage()
    {
        if (getDamage == null)
        {
            Debug.LogWarning($"GetDamage sound not found on {transform.name}");
        }
        else if (!getDamage.isPlaying)
        {
            getDamage.Play();
        }

        if (commonDamage == null)
        {
            Debug.LogWarning($"CommonDamage sound not found on {transform.name}");
        }
        else if (!commonDamage.isPlaying)
        {
            commonDamage.Play();
        }
    }

    public void PlayGetDamageSound()
    {
        if (getDamage == null)
        {
            Debug.LogWarning($"GetDamage sound not found on {transform.name}");
            return;
        }

        if (!getDamage.isPlaying)
        {
            getDamage.Play();
        }
    }

    public void GetShieldDamage()
    {
        shieldDamage.Play();
    }

    public void StartMove()
    {
        if (move == null)
        {
            Debug.LogWarning($"Move sound not found on {transform.name}");
            return;
        }

        if (!move.isPlaying)
        {
            move.Play();
        }
    }

    public void StopMove()
    {
        if (move == null)
        {
            Debug.LogWarning($"Move sound not found on {transform.name}");
            return;
        }

        move.Stop();
    }
}
