using DG.Tweening;
using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobUI : MonoBehaviour
{
    [SerializeField]
    private Cursor cursor;
    public Cursor MobCursor => cursor;

    [SerializeField]
    private HPBar hpBar;

    [SerializeField]
    private MobButtons buttons;

    [SerializeField]
    private MobText mobText;

    [SerializeField]
    private GameObject shieldIcon;
    [SerializeField]
    private float shieldScale;
    private Vector3 shieldOiriginPosition;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        hpBar.Init(GetComponent<Mob>().MobHP);
        shieldOiriginPosition = shieldIcon.transform.localPosition;
    }

    public void Activate()
    {
        buttons.ShowButtons();
        cursor.Activate();
    }

    public void HideUI()
    {
        MobCursor.Deactivate();
        buttons.HideButtons();
    }

    public void MobDeath()
    {
        buttons.HideButtons();
        hpBar.gameObject.SetActive(false);
        cursor.Hide();
    }

    public void ShowTargetCursor()
    {
        foreach (Mob mob in gameManager.EnemyMobs)
        {
            if (mob.IsHostile && !mob.IsDead)
            {
                mob.UI.MobCursor.ShowTarget();
            }
        }
    }

    public void UpdateHPBar(int value)
    {
        hpBar.UpdateHPBar(value);
    }

    public void ShowText(string value, Color color)
    {
        mobText.ShowText(value, color);
    }

    public void ShowShield()
    {
        DOTween.Sequence()
            .Append(shieldIcon.transform.DOScale(shieldScale, .3f))
            .Join(shieldIcon.transform.DOLocalMove(Vector3.zero, .5f))
            .AppendInterval(.3f)
            .OnComplete(GetComponent<Mob>().NextAction);
    }

    public void HideShield()
    {
        DOTween.Sequence()
            .Append(shieldIcon.transform.DOScale(Vector3.zero, .3f))
            .Join(shieldIcon.transform.DOLocalMove(shieldOiriginPosition, .5f));
    }
}
