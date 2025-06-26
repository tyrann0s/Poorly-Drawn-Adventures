using System;
using UnityEngine;
using UnityEngine.UI;

namespace Base.UI
{
    public class ScreenHub : BaseScreen
    {
        [SerializeField] private Button returnButton;

        private void Start()
        {
            returnButton.onClick.AddListener(OnReturnButtonClick);       
        }

        protected override void UpdateScreen()
        {
            throw new NotImplementedException();
        }
    }
}
