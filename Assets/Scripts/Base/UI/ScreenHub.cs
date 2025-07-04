using System;
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
            mobsPanel.UpdateMobPanel(ProgressManager.Instance.AvailableMobs);
        }

        protected override void OnReturnButtonClick()
        {
            foreach (MobElement element in currentTeamPanel.MobElements)
            {
                if (!element.MData)
                {
                    Debug.Log("КОМАНДА ДОЛЖА БЫТЬ ПОЛНОЙ");
                    return;
                }
            }
            
            ProgressManager.Instance.CurrentTeam.Clear();
            foreach (MobElement element in currentTeamPanel.MobElements)
            {
                ProgressManager.Instance.CurrentTeam.Add(element.MData);
            }
            
            base.OnReturnButtonClick();
        }

        protected override void UpdateScreen()
        {
            
        }
    }
}
