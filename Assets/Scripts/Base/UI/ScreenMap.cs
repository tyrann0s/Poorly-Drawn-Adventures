using System;
using Managers.Hub;
using UnityEngine;
using UnityEngine.UI;

namespace Base.UI
{
    public class ScreenMap : BaseScreen
    {
        [SerializeField] private Button returnButton;
        [SerializeField] private GameObject sceneLoaderObject;
        private AdvancedSceneLoader sceneLoader;

        private void Start()
        {
            sceneLoader = sceneLoaderObject.GetComponent<AdvancedSceneLoader>();
            returnButton.onClick.AddListener(OnReturnButtonClick);
        }

        public void LoadLevel(string level)
        {
            transform.parent.gameObject.SetActive(false);
            sceneLoader.LoadScene(level);       
        }

        protected override void UpdateScreen()
        {

        }
    }
}
