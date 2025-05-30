using UnityEngine;

namespace Mobs
{
    public class MobVFX : MobComponent
    {
        [SerializeField]
        private ParticleSystem deathVFX, getDamageVFX, shieldHitVFX;
        
        public void PlayDeathVFX()
        {
            if (deathVFX) deathVFX.Play();
            else Debug.Log("Death VFX is null");
        }
        
        public void PlayGetDamageVFX()
        {
            if (getDamageVFX) getDamageVFX.Play();
            else Debug.Log("Get Damage VFX is null");
        }
        
        public void PlayShieldHitVFX()
        {
            if (shieldHitVFX) shieldHitVFX.Play();
            else Debug.Log("Shield Hit VFX is null");
        }
    }
}
