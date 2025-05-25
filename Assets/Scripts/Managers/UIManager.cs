using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Cards;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private CurrentMobPanel currentMobPanel, enemyMobPanel;

    [SerializeField]
    private PopUpPanel gameEndPanel, anouncerPanel;

    [SerializeField]
    private Button startFightButton;

    [SerializeField]
    private Button changeCardButton;

    [SerializeField]
    private Button confirmChangeButton;

    [SerializeField] private Text combinationText;

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

    public void ShowChangeCardsButton()
    {
        DOTween.Sequence()
            .Append(changeCardButton.gameObject.transform.DOScale(.5f, .5f))
            .SetEase(Ease.InOutQuint);

        changeCardButton.GetComponentInChildren<Text>().text = "Change cards";
    }

    public void WaitChangeCardsButton()
    {
        changeCardButton.GetComponentInChildren<Text>().text = "Cancel";
    }

    public void HideChangeCardsButton()
    {
        DOTween.Sequence()
            .Append(changeCardButton.gameObject.transform.DOScale(0, .5f))
            .SetEase(Ease.InOutQuint);
    }

    public void ShowConfirmChangeButton()
    {
        DOTween.Sequence()
            .Append(confirmChangeButton.gameObject.transform.DOScale(.5f, .5f))
            .SetEase(Ease.InOutQuint);
    }

    public void HideConfirmChangeButton()
    {
        DOTween.Sequence()
            .Append(confirmChangeButton.gameObject.transform.DOScale(0, .5f))
            .SetEase(Ease.InOutQuint);
    }

    public void GameEndScreen()
    {
        gameEndPanel.gameObject.SetActive(true);
        currentMobPanel.gameObject.SetActive(false);
        enemyMobPanel.gameObject.SetActive(false);
        startFightButton.gameObject.SetActive(false);
    }

    public void ShowCombination(ElementCombo currentCombination)
    {
        combinationText.text = currentCombination == null ? "" : currentCombination.comboName;
    }
}
