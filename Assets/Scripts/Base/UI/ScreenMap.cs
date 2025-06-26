using System;
using Managers.Hub;
using UnityEngine;
using UnityEngine.UI;

namespace Base.UI
{
    public class ScreenMap : BaseScreen
    {
        [SerializeField] private Button returnButton;
        [SerializeField] private GameObject levelObject;

        private void Start()
        {
            returnButton.onClick.AddListener(OnReturnButtonClick);
        }

        protected override void OnReturnButtonClick()
        {
            base.OnReturnButtonClick();
            levelObject.SetActive(false);       
        }

        protected override void UpdateScreen()
        {
            levelObject.SetActive(true);
        }
    }
}
