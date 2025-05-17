using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MobButtons : MonoBehaviour
{
    [SerializeField]
    private MobButton skipTurnbutton, defenseButton, attack1Button, attack2Button;

    [SerializeField]
    private float animationSpeed;

    [SerializeField]
    private float scaleAmount;
    public float ScaleAmount => scaleAmount;

    private List<MobButton> buttons = new List<MobButton>();
    private Mob mob;

    private void Start()
    {
        mob = GetComponentInParent<Mob>();

        buttons.Add(skipTurnbutton);
        buttons.Add(defenseButton);
        buttons.Add(attack1Button);
        buttons.Add(attack2Button);

        foreach (MobButton button in buttons) { button.gameObject.SetActive(false); }
    }

    public void ShowButtons()
    {
        skipTurnbutton.SetActive(true, mob);

        if (mob.MobStamina < mob.MobData.DefenseCost) defenseButton.SetActive(false, mob);
        else defenseButton.SetActive(true, mob);

        if (mob.MobStamina < mob.MobData.Attack1Cost) attack1Button.SetActive(false, mob);
        else attack1Button.SetActive(true, mob);

        if (mob.MobStamina < mob.MobData.Attack2Cost) attack2Button.SetActive(false, mob);
        else attack2Button.SetActive(true, mob);

        foreach (MobButton button in buttons) { button.gameObject.SetActive(true); }
        DOTween.Sequence()
            .Append(skipTurnbutton.transform.DOLocalMove(skipTurnbutton.Destination(), animationSpeed))
            .Join(skipTurnbutton.transform.DOScale(.3f, animationSpeed))
            .Append(defenseButton.transform.DOLocalMove(defenseButton.Destination(), animationSpeed))
            .Join(defenseButton.transform.DOScale(.3f, animationSpeed))
            .Append(attack1Button.transform.DOLocalMove(attack1Button.Destination(), animationSpeed))
            .Join(attack1Button.transform.DOScale(.3f, animationSpeed))
            .Append(attack2Button.transform.DOLocalMove(attack2Button.Destination(), animationSpeed))
            .Join(attack2Button.transform.DOScale(.3f, animationSpeed));

    }

    public void HideButtons()
    {
        DOTween.Sequence()
            .Append(skipTurnbutton.transform.DOLocalMove(Vector3.zero, animationSpeed))
            .Join(skipTurnbutton.transform.DOScale(Vector3.zero, animationSpeed))
            .Append(defenseButton.transform.DOLocalMove(Vector3.zero, animationSpeed))
            .Join(defenseButton.transform.DOScale(Vector3.zero, animationSpeed))
            .Append(attack1Button.transform.DOLocalMove(Vector3.zero, animationSpeed))
            .Join(attack1Button.transform.DOScale(Vector3.zero, animationSpeed))
            .Append(attack2Button.transform.DOLocalMove(Vector3.zero, animationSpeed))
            .Join(attack2Button.transform.DOScale(Vector3.zero, animationSpeed))
            .OnComplete(DeactivateButtons);   
    }

    private void DeactivateButtons()
    {
        foreach (MobButton button in buttons) { button.gameObject.SetActive(false); }
    }
}
