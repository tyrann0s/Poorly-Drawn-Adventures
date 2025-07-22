using Cards;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Managers
{
    public class UIManager : MonoBehaviour, IManager
    {
        [SerializeField]
        private PopUpPanel gameEndPanel;

        [SerializeField]
        private PopUpPanel announcerPanel;

        [SerializeField]
        private Button assignActionsButton;
        
        [SerializeField]
        private Button startFightButton;

        [SerializeField]
        private Button changeCardButton;
        private Text changeCardButtonText;

        [SerializeField]
        private Button confirmChangeButton;

        [SerializeField] private Text combinationText;
        [SerializeField] private Text coinsText;
    
        public UISounds UISounds { get; private set; }
        
        public void Initialize()
        {
            UISounds = GetComponentInChildren<UISounds>();
            changeCardButtonText = changeCardButton.GetComponentInChildren<Text>();
        }

        public void ShowAnnouncerPanel(bool isAnimated, string value)
        {
            if (isAnimated) announcerPanel.ShowAnimated(value);
            else announcerPanel.Show(value);
        }

        public void ShowGameEndPanel(string value)
        {
            gameEndPanel.Show(value);
        }

        public void ShowAssignActionsButton()
        {
            DOTween.Sequence()
                .Append(assignActionsButton.gameObject.transform.DOScale(1, .5f))
                .SetEase(Ease.InOutQuint);
        }

        public void HideAssignActionsButton()
        {
            DOTween.Sequence()
                .Append(assignActionsButton.gameObject.transform.DOScale(0, .5f))
                .SetEase(Ease.InOutQuint);
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
            SetChangeCardButtonText(true);
            DOTween.Sequence()
                .Append(changeCardButton.gameObject.transform.DOScale(.5f, .5f))
                .SetEase(Ease.InOutQuint);
        }

        public void SetChangeCardButtonText(bool value)
        {
            if (value) changeCardButtonText.text = "Change Cards";
            else changeCardButtonText.text = "Cancel";
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
            if (confirmChangeButton != null && confirmChangeButton.gameObject != null)
            {
                DOTween.Sequence()
                    .Append(confirmChangeButton.gameObject.transform.DOScale(0, .5f))
                    .SetEase(Ease.InOutQuint);
            }
        }

        public void GameEndScreen()
        {
            gameEndPanel.gameObject.SetActive(true);
            startFightButton.gameObject.SetActive(false);
        }

        public void ShowCombination(string text)
        {
            combinationText.text = text;
        }
    
        private void OnDestroy()
        {
            // Останавливает все твины, связанные с этим объектом
            DOTween.Kill(gameObject);
        }

        public void UpdateCoins(float value)
        {
            coinsText.text = value.ToString();
        }
    }
}