using Cards;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager instance;
        public static UIManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindFirstObjectByType<UIManager>();
                    if (instance == null)
                    {
                        GameObject go = new GameObject("UI Manager");
                        instance = go.AddComponent<UIManager>();
                    }
                }
                return instance;
            }
        }
    
        [SerializeField]
        private PopUpPanel gameEndPanel, anouncerPanel;

        [SerializeField]
        private Button startFightButton;

        [SerializeField]
        private Button changeCardButton;

        [SerializeField]
        private Button confirmChangeButton;

        [SerializeField] private Text combinationText;
        [SerializeField] private Text coinsText;
    
        public UISounds UISounds { get; private set; }

        private void Start()
        {
            UISounds = GetComponentInChildren<UISounds>();
        }

        public void ShowAnouncerPanel(bool isAnimated, string value)
        {
            if (isAnimated) anouncerPanel.ShowAnimated(value);
            else anouncerPanel.Show(value);
        }

        public void ShowGameEndPanel(string value)
        {
            gameEndPanel.Show(value);
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

        public void ShowCombination(ElementCombo currentCombination)
        {
            combinationText.text = currentCombination == null ? "" : currentCombination.comboName;
        }
    
        private void OnDestroy()
        {
            // Останавливает все твины, связанные с этим объектом
            DOTween.Kill(transform);
        }

        public void UpdateCoins(float value)
        {
            coinsText.text = value.ToString();
        }
    }
}