using System;
using Managers;
using UnityEngine;

namespace UI.Menu
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private GameObject menuBKG, fleeButtons, exitButtons, settingsPanel;
        private Journal journal;
        
        private void Start()
        {
            journal = GetComponentInParent<Journal>();
        }

        public void Resume()
        {
            journal.HideMenu();
        }

        public void ShowFleeConfirm()
        {
            menuBKG.SetActive(true);
            fleeButtons.SetActive(true);
        }
        
        public void ShowExitConfirm()
        {
            menuBKG.SetActive(true);
            exitButtons.SetActive(true);
        }

        public void ShowSetings()
        {
            menuBKG.SetActive(true);
            settingsPanel.SetActive(true);
        }
        
        public void HideSetings()
        {
            menuBKG.SetActive(false);
            settingsPanel.SetActive(false);
        }

        public void CancelConfirm()
        {
            menuBKG.SetActive(false);
            fleeButtons.SetActive(false);
            exitButtons.SetActive(false);
        }

        public void Flee()
        {
            ServiceLocator.Get<GameManager>().BackToBase();
        }
        
        public void Exit()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif

        }
    }
}
