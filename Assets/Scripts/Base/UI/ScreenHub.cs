using System;
using Managers;
using Managers.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Base.UI
{
    public class ScreenHub : BaseScreen
    {
        [SerializeField] private Button returnButton;
        [SerializeField] private CurrentTeamPanel currentTeamPanel;
        [SerializeField] private MobListPanel heroesPanel, mobsPanel;

        private void Start()
        {
            returnButton.onClick.AddListener(OnReturnButtonClick);      
            currentTeamPanel.UpdateTeamPanel();
            heroesPanel.UpdateMobPanel(ProgressManager.Instance.AvailableHeroes);
            mobsPanel.UpdateMobPanel(ProgressManager.Instance.AvailableAllies);
        }

        protected override void OnReturnButtonClick()
        {
            ProgressManager.Instance.CurrentTeam.Clear();
            foreach (MobElement element in currentTeamPanel.MobElements)
            {
                if (element.MData) ProgressManager.Instance.CurrentTeam.Add(element.MData);
            }
            
            if (!ServiceLocator.Get<BaseManager>().CheckIfTeamIsFull()) return;
            
            base.OnReturnButtonClick();
        }

        protected override void UpdateScreen()
        {
            
        }
    }
}
