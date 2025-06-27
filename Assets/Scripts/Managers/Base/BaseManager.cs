using System;
using System.Collections.Generic;
using Base.UI;
using Managers.Hub;
using Mobs;
using UnityEngine;

namespace Managers.Base
{
    public class BaseManager : MonoBehaviour
    {
        public static BaseManager Instance { get; private set; }
        
        [SerializeField] private BaseScreen screenMain, screenMap, screenHub;
        
        public List<MobData> CurrentTeam { get; set; } = new();
        public List<MobData> testMobs = new();

        private void Awake()
        {
            if (Instance && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            
            CurrentTeam = testMobs;
        }

        private void Start()
        {
            ShowMainScreen();
        }

        public void ShowMainScreen()
        {
            screenMain.ShowScreen();
            screenMap.HideScreen();
            screenHub.HideScreen();
        }
        
        public void ShowMapScreen()
        {
            screenMain.HideScreen();
            screenMap.ShowScreen();
            screenHub.HideScreen();
        }
        
        public void ShowHubScreen()
        {
            screenMain.HideScreen();
            screenMap.HideScreen();
            screenHub.ShowScreen();
        }
    }
}
