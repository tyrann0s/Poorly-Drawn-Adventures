using Cards;
using DG.Tweening;
using UnityEngine;
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
        private Button changeCardButton, confirmChangeButton, cancelChangeButton;

        [SerializeField] private Text combinationText;
        [SerializeField] private Text coinsText;
    
        public UISounds UISounds { get; private set; }
        
        public void Initialize()
        {
            UISounds = GetComponentInChildren<UISounds>();
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

        public void PrepareChangeCards()
        {
            changeCardButton.gameObject.SetActive(true);
            confirmChangeButton.gameObject.SetActive(false);
            cancelChangeButton.gameObject.SetActive(false);
        }

        public void EnterChangeCards()
        {
            ServiceLocator.Get<CardPanel>().CardChangeMode = true;
            ServiceLocator.Get<CardPanel>().EnableInteraction();
            changeCardButton.gameObject.SetActive(false);
            confirmChangeButton.gameObject.SetActive(true);
            SetConfirmChangeButton(false);
            cancelChangeButton.gameObject.SetActive(true);
        }
        
        public void ExitChangeCards()
        {
            ServiceLocator.Get<CardPanel>().CardChangeMode = false;
            ServiceLocator.Get<CardPanel>().DisableInteraction();
            if (ServiceLocator.Get<CardPanel>().CanChangeCards() == false) ServiceLocator.Get<CardPanel>().HideButtons();
            
            changeCardButton.gameObject.SetActive(true);
            confirmChangeButton.gameObject.SetActive(false);
            cancelChangeButton.gameObject.SetActive(false);
        }

        public void SetConfirmChangeButton(bool isInteractable)
        {
            confirmChangeButton.interactable = isInteractable;
        }

        public void GameEndScreen()
        {
            gameEndPanel.gameObject.SetActive(true);
            startFightButton.gameObject.SetActive(false);
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