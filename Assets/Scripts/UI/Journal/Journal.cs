using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Journal : MonoBehaviour
    {
        [SerializeField] private GameObject mainWindow;
        [SerializeField] private GameObject openButton;
        
        [SerializeField] private GameObject pageButtonPrefab;
        [SerializeField] private Transform subPageContainer;
        [SerializeField] private RecordsPanel recordsPanel;

        public void ShowJournal()
        {
            openButton.SetActive(false);  
            mainWindow.SetActive(true);
            AddComboButtons();
        }
        
        public void HideJournal()
        {
            mainWindow.SetActive(false);  
            openButton.SetActive(true);       
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
    }
}
