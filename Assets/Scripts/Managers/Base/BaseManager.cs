using Base.UI;
using UnityEngine;

namespace Managers.Base
{
    public class BaseManager : MonoBehaviour
    {
        public static BaseManager Instance { get; private set; }
        
        [SerializeField] private BaseScreen screenMain, screenMap, screenHub;
        [SerializeField] private CoinsPanel coinsPanel;
        
        private void Awake()
        {
            if (Instance && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            ShowMainScreen();
            UpdateCoinsPanel(ProgressManager.Instance.Coins.ToString());
        }

        public void ShowMainScreen()
        {
            screenMain.ShowScreen();
            screenMap.HideScreen();
            screenHub.HideScreen();
        }
        
        public void ShowMapScreen()
        {
            screenMain.HideScreen();
            screenMap.ShowScreen();
            screenHub.HideScreen();
        }
        
        public void ShowHubScreen()
        {
            screenMain.HideScreen();
            screenMap.HideScreen();
            screenHub.ShowScreen();
        }

        public void SpendCoins(int amount)
        {
            ProgressManager.Instance.Coins -= amount;
            UpdateCoinsPanel(ProgressManager.Instance.Coins.ToString());       
        }
        
        private void UpdateCoinsPanel(string text)
        {
            coinsPanel.UpdateText(text);
        }
        
        public bool CheckIfTeamIsFull()
        {
            if (ProgressManager.Instance.CurrentTeam.Count < 4)
            {
                Debug.Log("КОМАНДА ДОЛЖА БЫТЬ ПОЛНОЙ");
                return false;
            }
            
            return true;
        }
    }
}
