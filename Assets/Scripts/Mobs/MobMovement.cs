using DG.Tweening;
using UnityEngine;

namespace Mobs
{
    public class MobMovement : MobComponent
    {
        [SerializeField]
        private GameObject rivalPosition;
        public GameObject RivalPosition => rivalPosition;
        public Vector3 OriginPosition { get; set; }
        
        private SpriteRenderer spriteRenderer;

        protected override void Awake()
        {
            base.Awake();
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
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
            if (transform.position == OriginPosition) return;

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

        public void MoveToEnemy()
        {
            DOTween.Sequence()
                .Append(transform.DOMove(ParentMob.CurrentAction.TargetInstance.MobMovement.RivalPosition.transform.position, 1f))
                .OnComplete(ParentMob.AnimationController.PlayActionAnimation);
        }

        public void MoveToRivalPosition()
        {
            transform.DOMove(rivalPosition.transform.position, .3f);
        }

        private void OnDestroy()
        {
            // Останавливает все твины, связанные с этим объектом
            DOTween.Kill(transform);
        }
    }
}
