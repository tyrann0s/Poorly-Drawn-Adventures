using DG.Tweening;
using UnityEngine;

namespace Mobs
{
    public class MobMovement : MobComponent
    {
        [SerializeField]
        private GameObject rivalPosition;
        public GameObject RivalPosition => rivalPosition;
        public Vector3 ReadyPosition { get; set; }
        public Vector3 OriginPosition { get; set; }
        
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        protected override void Awake()
        {
            base.Awake();
            if (!spriteRenderer)
            {
                Debug.LogError($"SpriteRenderer not found on {name}");
            }
        }

        private void Start()
        {
            if (ParentMob.IsHostile) rivalPosition.transform.localPosition = new Vector3(-rivalPosition.transform.localPosition.x, 0, 0);
        }
        
        public void MirrorMob()
        {
            spriteRenderer.flipX = true;
        }
        
        public void GoToOriginPosition(bool withSound)
        {
            if (ParentMob.State == MobState.Dead) return;
            if (IsOnOriginPosition()) return;

            ParentMob.ChangeLane(ParentMob.OriginLane);
            FlipMob();
            if (withSound) ParentMob.SoundController.StartMove();
            ParentMob.AnimationController.PlayRun_Animation();
            DOTween.Sequence()
                .Append(transform.DOMove(OriginPosition, 1f))
                .OnComplete(FlipMob);
        }

        private void FlipMob()
        {
            if (spriteRenderer.flipX) spriteRenderer.flipX = false;
            else spriteRenderer.flipX = true;

            ParentMob.SoundController.StopMove();
            ParentMob.AnimationController.PlayIdle_Animation();
        }

        public void MoveToEnemy(bool isActive)
        {
            ParentMob.ChangeLane(ParentMob.CurrentAction.TargetInstance.OriginLane);
            DOTween.Sequence()
                .Append(transform.DOMove(ParentMob.CurrentAction.TargetInstance.MobMovement.RivalPosition.transform.position, 1f))
                .OnComplete(OnMoveComplete(isActive));
        }

        private TweenCallback OnMoveComplete(bool isActive)
        {
            return () =>
            {
                ParentMob.AnimationController.PlayActionAnimation(isActive);
            };
        }

        public void MoveToRivalPosition()
        {
            transform.DOMove(rivalPosition.transform.position, .5f);
        }
        
        public void MoveToReadyPosition()
        {
            transform.DOMove(ReadyPosition, .5f);
        }
        
        public bool IsOnOriginPosition()
        {
            return transform.position == OriginPosition;
        }
    }
}
