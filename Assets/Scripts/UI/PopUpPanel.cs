using DG.Tweening;
using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class PopUpPanel : MonoBehaviour
{
    [SerializeField]
    private Text text;

    [SerializeField]
    private float delayTime;

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
        GameManager.Instance.ControlLock = true;
    }

    private void ControlLockOff()
    {
        GameManager.Instance.ControlLock = false;
    }
    
    private void OnDestroy()
    {
        // Останавливает все твины, связанные с этим объектом
        DOTween.Kill(transform);
    }
}
