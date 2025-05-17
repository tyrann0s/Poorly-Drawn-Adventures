using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MobText : MonoBehaviour
{
    [SerializeField]
    private TMP_Text text;

    [SerializeField]
    private float animationSpeed;

    [SerializeField]
    private Transform destination;

    public void ShowText(string value, Color color)
    {
        text.text = value;
        text.color = color;

        DOTween.Sequence()
            .Append(transform.DOLocalMove(destination.localPosition, animationSpeed))
            .Join(transform.DOScale(.5f, animationSpeed))
            .AppendInterval(animationSpeed)
            .Append(text.DOFade(0f, animationSpeed))
            .Append(transform.DOLocalMove(Vector3.zero, 0))
            .Append(transform.DOScale(Vector3.zero, 0))
            .Append(text.DOFade(1, 1));
    }
}
