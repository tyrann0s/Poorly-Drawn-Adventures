using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MobButton : MonoBehaviour
{
    [SerializeField]
    private float destinationX, destinationY;
    public Vector3 Destination() { return new Vector3(destinationX, destinationY, 0); }

    private MobButtons buttons;

    private bool isActive;

    private Mob mob;

    [SerializeField]
    private TMP_Text text;

    [SerializeField]
    private UnityEvent action;

    private void Start()
    {
        buttons = GetComponentInParent<MobButtons>();
    }

    private void OnMouseEnter()
    {
        if (isActive)
        {
            transform.DOScale(buttons.ScaleAmount + .2f, .3f);
            FindObjectOfType<UISounds>().ButtonHover();
        }
    }

    private void OnMouseExit()
    {
        if (isActive) transform.DOScale(buttons.ScaleAmount, .3f);
    }

    private void OnMouseDown()
    {
        if (isActive)
        {
            buttons.HideButtons();
            action.Invoke();
            FindObjectOfType<UISounds>().ButtonClick();
        }
        else mob.UI.ShowText("NOT ENOUGH STAMINA", Color.white);
    }

    public void SetActive(bool value, Mob targetMob)
    {
        if (value)
        {
            text.color = Color.black;
        }
        else text.color = Color.gray;

        isActive = value;
        mob = targetMob;
    }
}
