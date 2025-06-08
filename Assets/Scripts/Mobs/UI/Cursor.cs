using DG.Tweening;
using UnityEngine;

namespace Mobs
{
    public class Cursor : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;

        private Tween pulseTween;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = false;
        }

        public void Show()
        {
            spriteRenderer.enabled = true;
        }

        public void ShowTarget()
        {
            spriteRenderer.color = Color.red;
            spriteRenderer.enabled = true;
            ZoomOut();
        }

        public void Hide()
        {
            if (spriteRenderer != null) spriteRenderer.enabled = false;
        }

        public void HideTarget()
        {
            spriteRenderer.color = Color.white;
            spriteRenderer.enabled = false;
        }

        public void Activate()
        {
            Show();
            spriteRenderer.color = Color.green;
            PulseTween();
        }

        public void Deactivate()
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = new Color(1f, 1f, 1f, 0f); // Сделать прозрачным
            }
        }

        public void PulseTween()
        {
            pulseTween = DOTween.Sequence()
                .Append(transform.DOScale(1.1f, .3f))
                .Append(transform.DOScale(1, .3f))
                .SetLoops(-1);
        }

        public void ZoomIn()
        {
            transform.DOScale(1f, .3f);
        }

        public void ZoomOut()
        {
            transform.DOScale(.7f, .3f);
        }

        public void PickTarget()
        {
            DOTween.Sequence()
                .Append(transform.DOScale(0f, .3f))
                .OnComplete(Deactivate);
        }
    
        private void OnDestroy()
        {
            // Останавливает все твины, связанные с этим объектом
            DOTween.Kill(transform);
        }
    }
}