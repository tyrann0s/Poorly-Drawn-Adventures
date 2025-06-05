using UnityEngine;

namespace Mobs
{
    public class AnimationController : MobComponent
    {
        private Animator animator;

        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
        }

        public void PlayIdle_Animation()
        {
            animator.Play("Idle");
        }

        public void PlayRun_Animation()
        {
            animator.Play("Run");
        }

        public void PlayAttack1_Animation()
        {
            animator.Play("Attack1");
        }

        public void PlayAttack2_Animation()
        {
            animator.Play("Attack2");
        }

        public void PlayGetDamage_Animation()
        {
            animator.Play("GetDamage");
        }

        public void PlayDie_Animation()
        {
            animator.Play("Die");
        }
    
        public void PlayAttackAnimation()
        {
            ParentMob.SoundController.StopMove();
            switch (ParentMob.CurrentAction.MobActionType)
            {
                case ActionType.Attack:
                    ParentMob.AnimationController.PlayAttack1_Animation();
                    ParentMob.SoundController.Attack1();
                    break;

                case ActionType.Skill:
                    ParentMob.AnimationController.PlayAttack2_Animation();
                    ParentMob.SoundController.Attack2();
                    break;
            }
        }
    }
}
