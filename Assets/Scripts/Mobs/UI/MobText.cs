using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Mobs
{
    public class MobText : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text text;

        [SerializeField]
        private float animationSpeed;
        
        private Vector3 originalPosition;

        private void Start()
        {
            originalPosition = transform.localPosition;
        }

        public void ShowText(string value, Color color)
        {
            text.text = value;
            text.color = color;

            DOTween.Sequence()
                .Append(transform.DOLocalMoveY(originalPosition.y + .5f, animationSpeed))
                .Join(transform.DOScale(.5f, animationSpeed))
                .AppendInterval(animationSpeed)
                .Append(text.DOFade(0f, animationSpeed))
                .Append(transform.DOLocalMove(originalPosition, 0))
                .Append(transform.DOScale(Vector3.zero, 0))
                .Append(text.DOFade(1, 1));
        }
    
        private void OnDestroy()
        {
            // Останавливает все твины, связанные с этим объектом
            DOTween.Kill(transform);
        }
    }
}
