using UnityEngine;
using UnityEngine.Serialization;

namespace Mobs
{
    public class SoundController : MobComponent
    {
        [SerializeField]
        private AudioSource pick;

        [SerializeField]
        private AudioSource attack, skill, death, souls, getDamage, commonDamage, shieldDamage, move;

        public void Pick()
        {
            if (!pick)
            {
                Debug.LogWarning($"Pick sound not found on {transform.name}");
                return;
            }

            if (!pick.isPlaying)
            {
                pick.Play();
            }
        }

        public void Attack()
        {
            if (!attack)
            {
                Debug.LogWarning($"Attack1 sound not found on {transform.name}");
                return;
            }

            if (!attack.isPlaying)
            {
                attack.Play();
            }
        }

        public void Skill()
        {
            if (!skill)
            {
                Debug.LogWarning($"Attack2 sound not found on {transform.name}");
                return;
            }

            if (!skill.isPlaying)
            {
                skill.Play();
            }
        }

        public void PlayShieldDamageSound()
        {
            if (!shieldDamage)
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
            if (!commonDamage)
            {
                Debug.LogWarning($"CommonDamage sound not found on {transform.name}");
            }
            else if (!commonDamage.isPlaying)
            {
                commonDamage.Play();
            }

            if (!death)
            {
                Debug.LogWarning($"Death sound not found on {transform.name}");
            }
            else if (!death.isPlaying)
            {
                death.Play();
            }

            if (!souls)
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
            if (!getDamage)
            {
                Debug.LogWarning($"GetDamage sound not found on {transform.name}");
            }
            else if (!getDamage.isPlaying)
            {
                getDamage.Play();
            }

            if (!commonDamage)
            {
                Debug.LogWarning($"CommonDamage sound not found on {transform.name}");
            }
            else if (!commonDamage.isPlaying)
            {
                commonDamage.Play();
            }
        }

        public void PlayGetDamageSound(bool isCritical)
        {
            if (!getDamage)
            {
                Debug.LogWarning($"GetDamage sound not found on {transform.name}");
                return;
            }

            if (!getDamage.isPlaying)
            {
                if (isCritical)
                {
                    getDamage.Play();
                }
            }
        }

        public void GetShieldDamage()
        {
            shieldDamage.Play();
        }

        public void StartMove()
        {
            if (!move)
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
            if (!move)
            {
                Debug.LogWarning($"Move sound not found on {transform.name}");
                return;
            }

            move.Stop();
        }
    }
}
