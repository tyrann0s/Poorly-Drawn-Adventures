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
        if (gameManager == null)
        {
            Debug.LogError($"Game Manager not found on {transform.name}");
        }

        if (cursor == null)
        {
            Debug.LogError($"Cursor not found on {transform.name}");
        }

        if (hpBar == null)
        {
            Debug.LogError($"HP Bar not found on {transform.name}");
        }

        if (buttons == null)
        {
            Debug.LogError($"Buttons not found on {transform.name}");
        }

        if (mobText == null)
        {
            Debug.LogError($"MobText not found on {transform.name}");
        }

        if (shieldIcon == null)
        {
            Debug.LogError($"Shield icon not found on {transform.name}");
        }
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
        if (gameManager == null)
        {
            Debug.LogError($"Game Manager is null in {transform.name}");
            return;
        }

        if (gameManager.EnemyMobs == null)
        {
            Debug.LogError($"EnemyMobs list is null in {transform.name}");
            return;
        }

        foreach (Mob mob in gameManager.EnemyMobs)
        {
            if (mob == null || !mob.IsHostile || mob.IsDead)
                continue;

            if (mob.UI != null && mob.UI.MobCursor != null)
            {
                mob.UI.MobCursor.ShowTarget();
            }
            else
            {
                Debug.LogError($"UI or Cursor is null on enemy mob {mob.name}");
            }
        }
    }

    public void UpdateHP(float value)
    {
        if (hpBar != null)
        {
            hpBar.UpdateHP(value);
        }
    }

    public void UpdateMaxHP(int value)
    {
        if (hpBar != null)
        {
            hpBar.UpdateMaxHP(value);
        }
    }

    public void ShowText(string value, Color color)
    {
        mobText.ShowText(value, color);
    }

    public void ShowShield()
    {
        if (shieldIcon == null)
        {
            Debug.LogError($"Shield icon is null in {transform.name}");
            return;
        }

        DOTween.Sequence()
            .Append(shieldIcon.transform.DOScale(shieldScale, .3f))
            .Join(shieldIcon.transform.DOLocalMove(Vector3.zero, .5f))
            .AppendInterval(.3f)
            .OnComplete(() =>
            {
                var mob = GetComponent<Mob>();
                if (mob != null)
                {
                    mob.NextAction();
                }
                else
                {
                    Debug.LogError($"Mob component is null in {transform.name}");
                }
            });
    }

    public void HideShield()
    {
        DOTween.Sequence()
            .Append(shieldIcon.transform.DOScale(Vector3.zero, .3f))
            .Join(shieldIcon.transform.DOLocalMove(shieldOiriginPosition, .5f));
    }
}
