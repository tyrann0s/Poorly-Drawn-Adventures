using System;
using Managers;
using Managers.Base;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class Journal : MonoBehaviour
    {
        [SerializeField] private GameObject journalWindow, menuWindow;
        [SerializeField] private GameObject journalButton, menuButton;
        [SerializeField] private GameObject background;
        
        [SerializeField] private GameObject pageButtonPrefab;
        [SerializeField] private Transform subPageContainer;
        [SerializeField] private RecordsPanel recordsPanel;
        
        [SerializeField] private NotificationPanel notificationPanelPrefab;
        [SerializeField] private Transform notifications;

        private void OnEnable()
        {
            ProgressManager.Instance.OnRecordChanged += ShowNotification;
        }
        
        private void OnDestroy()
        {
            ProgressManager.Instance.OnRecordChanged -= ShowNotification;
        }

        public void ShowJournal()
        {
            PauseGame();
            
            journalButton.SetActive(false);  
            menuButton.SetActive(false);
            
            journalWindow.SetActive(true);
            background.SetActive(true);
            AddComboButtons();
        }
        
        public void HideJournal()
        {
            ResumeGame();
            
            journalWindow.SetActive(false);
            background.SetActive(false);
            
            journalButton.SetActive(true);  
            menuButton.SetActive(true);       
        }

        public void ShowMenu()
        {
            PauseGame();
            
            journalButton.SetActive(false);  
            menuButton.SetActive(false);  
            
            menuWindow.SetActive(true);
            background.SetActive(true);
        }

        public void HideMenu()
        {
            ResumeGame();
            
            menuWindow.SetActive(false);
            background.SetActive(false);
            
            journalButton.SetActive(true);  
            menuButton.SetActive(true);         
        }

        public void AddComboButtons()
        {
            ClearButtons();
            recordsPanel.ClearRecords();
            
            CreateButton().Initialize("Elemental", recordsPanel.ShowElementalCombos);
            CreateButton().Initialize("Rank", recordsPanel.ShowRankCombos);
            CreateButton().Initialize("Special", recordsPanel.ShowSpecialCombos);
            
            recordsPanel.ShowElementalCombos();
        }

        public void AddMobsButtons()
        {
            ClearButtons();
            recordsPanel.ClearRecords();
            
            CreateButton().Initialize("Enemies", recordsPanel.ShowEnemies);
            CreateButton().Initialize("Bosses", recordsPanel.ShowBosses);
            CreateButton().Initialize("Allies", recordsPanel.ShowAllies);
            CreateButton().Initialize("Heroes", recordsPanel.ShowHeroes);
            
            recordsPanel.ShowEnemies();
        }

        private void ClearButtons()
        {
            foreach (Transform child in subPageContainer)
            {
                Destroy(child.gameObject);
            }
        }
        
        private JournalPageButton CreateButton()
        {
            GameObject elementButtonGO = Instantiate(pageButtonPrefab, subPageContainer);
            return elementButtonGO.GetComponent<JournalPageButton>();
        }
        
        private void ShowNotification(string text)
        {
            var notificationPanel = Instantiate(notificationPanelPrefab, notifications);
            notificationPanel.ShowNotification(text);
        }

        private void PauseGame()
        {
            Time.timeScale = 0;
        }
        
        private void ResumeGame()
        {
            Time.timeScale = 1;       
        }
    }
}
