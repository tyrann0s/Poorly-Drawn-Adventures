using System;
using UnityEngine;

namespace Mobs
{
    public class AnimationController : MobComponent
    {
        [SerializeField] private Animator animator;

        protected override void Awake()
        {
            base.Awake();
        }

        public void PlayIdle_Animation()
        {
            animator.Play("Idle");
        }

        public void PlayRun_Animation()
        {
            animator.Play("Move");
        }

        public void PlayAttack_Animation()
        {
            animator.Play("Attack");
        }

        private void PlayActiveSkill_Animation()
        {
            animator.Play("Active Skill");
        }

        private void PlayPassiveSkill_Animation()
        {
            animator.Play("Passive Skill");
        }

        public void PlayGetDamage_Animation()
        {
            animator.Play("Damage");
        }

        public void PlayDie_Animation()
        {
            animator.Play("Death");
        }
    
        public void PlayActionAnimation(bool isActive)
        {
            if (ParentMob.State == MobState.Dead) return;
            
            ParentMob.SoundController.StopMove();

            if (isActive)
            {
                switch (ParentMob.CurrentAction.MobActionType)
                {
                    case ActionType.Attack:
                        PlayAttack_Animation();
                        ParentMob.SoundController.Attack();
                        break;

                    case ActionType.ActiveSkill:
                        PlayActiveSkill_Animation();
                        ParentMob.SoundController.ActiveSkill();
                        break;
                }
            }
            else
            {
                PlayPassiveSkill_Animation();
                ParentMob.SoundController.ActiveSkill();
            }
        }
    }
}
