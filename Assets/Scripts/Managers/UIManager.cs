using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private CurrentMobPanel currentMobPanel, enemyMobPanel;

    [SerializeField]
    private PopUpPanel gameEndPanel, anouncerPanel;

    [SerializeField]
    private Button startFightButton;

    public void ShowAnouncerPanel(bool isAnimated, string value)
    {
        if (isAnimated) anouncerPanel.ShowAnimated(value);
        else anouncerPanel.Show(value);
    }

    public void ShowGameEndPanel(string value)
    {
        gameEndPanel.Show(value);
    }

    public void UpdateCurrentMobPanel(Mob mob)
    {
        UpdateMobPanel(currentMobPanel, mob);
    }

    public void UpdateEnemyMobPanel(Mob mob)
    {
        UpdateMobPanel(enemyMobPanel, mob);
    }

    private void UpdateMobPanel(CurrentMobPanel mobPanel, Mob mob)
    {
        if (mob != null)
        {
            mobPanel.UpdatePanel(mob);
        }
        else
        {
            mobPanel.UpdatePanel();
        }
    }

    public void ShowStartBattleButton()
    {
        DOTween.Sequence()
            .Append(startFightButton.gameObject.transform.DOScale(1, .5f))
            .SetEase(Ease.InOutQuint);
    }

    public void HideStartBattleButton()
    {
        DOTween.Sequence()
            .Append(startFightButton.gameObject.transform.DOScale(0, .5f))
            .SetEase(Ease.InOutQuint);
    }

    public void GameEndScreen()
    {
        gameEndPanel.gameObject.SetActive(true);
        currentMobPanel.gameObject.SetActive(false);
        enemyMobPanel.gameObject.SetActive(false);
        startFightButton.gameObject.SetActive(false);
    }
}
