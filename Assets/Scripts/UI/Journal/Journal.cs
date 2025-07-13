using System;
using UnityEngine;

namespace UI
{
    public class Journal : MonoBehaviour
    {
        [SerializeField] private GameObject pageButtonPrefab;
        [SerializeField] private Transform subPageContainer;
        [SerializeField] private RecordsPanel recordsPanel;

        private void Start()
        {
            AddComboButtons();
        }

        public void AddComboButtons()
        {
            ClearButtons();
            
            CreateButton().Initialize("Elemental", recordsPanel.ShowElementalCombos);
            CreateButton().Initialize("Rank", recordsPanel.ShowRankCombos);
            CreateButton().Initialize("Special", recordsPanel.ShowSpecialCombos);
        }

        public void AddMobsButtons()
        {
            ClearButtons();
            
            CreateButton().Initialize("Enemies", recordsPanel.ShowEnemies);
            CreateButton().Initialize("Bosses", recordsPanel.ShowBosses);
            CreateButton().Initialize("Allies", recordsPanel.ShowAllies);
            CreateButton().Initialize("Heroes", recordsPanel.ShowHeroes);
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
