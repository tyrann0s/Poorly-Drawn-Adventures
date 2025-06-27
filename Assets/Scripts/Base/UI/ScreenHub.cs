using System;
using UnityEngine;
using UnityEngine.UI;

namespace Base.UI
{
    public class ScreenHub : BaseScreen
    {
        [SerializeField] private Button returnButton;
        [SerializeField] private CurrentTeamPanel currentTeamPanel;

        private void Start()
        {
            returnButton.onClick.AddListener(OnReturnButtonClick);       
        }

        protected override void UpdateScreen()
        {
            currentTeamPanel.UpdateTeamPanel();
        }
    }
}
