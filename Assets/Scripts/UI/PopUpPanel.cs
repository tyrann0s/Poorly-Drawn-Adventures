using DG.Tweening;
using DG.Tweening.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpPanel : MonoBehaviour
{
    [SerializeField]
    private Text text;

    [SerializeField]
    private float delayTime;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void Show(string value)
    {
        text.text = value;
    }

    public void ShowAnimated(string value)
    {
        text.text = value;
        StartCoroutine(TempShow());
    }

    private IEnumerator TempShow()
    {
        DOTween.Sequence()
            .Append(transform.DOScale(1, .2f))
            .OnComplete(ControlLockOn);

        yield return new WaitForSeconds(delayTime);

        DOTween.Sequence()
            .Append(transform.DOScale(0, .2f))
            .OnComplete(ControlLockOff);
    }

    private void ControlLockOn()
    {
        gameManager.ControlLock = true;
    }

    private void ControlLockOff()
    {
        gameManager.ControlLock = false;
    }
}
