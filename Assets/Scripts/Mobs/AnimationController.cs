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

        public void PlayAttack_Animation()
        {
            animator.Play("Attack1");
        }

        public void PlaySkill_Animation()
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
                    ParentMob.AnimationController.PlayAttack_Animation();
                    ParentMob.SoundController.Attack();
                    break;

                case ActionType.Skill:
                    ParentMob.AnimationController.PlaySkill_Animation();
                    ParentMob.SoundController.Skill();
                    break;
            }
        }
    }
}
