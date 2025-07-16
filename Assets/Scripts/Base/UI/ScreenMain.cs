using System;
using Managers.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Base.UI
{
    public class ScreenMain: BaseScreen
    {
        [SerializeField] private Button hubButton, mapButton;
        [SerializeField] private CurrentTeamPanel currentTeamPanel;

        private void Start()
        {
            hubButton.onClick.AddListener(OnHubButtonClick);
            mapButton.onClick.AddListener(OnMapButtonClick);
        }
        
        private void OnHubButtonClick()
        {
            ServiceLocator.Get<BaseManager>().ShowHubScreen();
        }
        
        private void OnMapButtonClick()
        {
            ServiceLocator.Get<BaseManager>().ShowMapScreen();       
        }

        protected override void UpdateScreen()
        {
            currentTeamPanel.UpdateTeamPanel();
        }
    }
}
