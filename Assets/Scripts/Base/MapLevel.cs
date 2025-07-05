using System.Collections.Generic;
using Base.UI;
using Hub.UI;
using Levels;
using Managers.Base;
using TMPro;
using UnityEngine;

namespace Hub
{
    public class MapLevel : MonoBehaviour
    {
        [SerializeField] private Level level;
        [SerializeField] private SpriteRenderer iconSprite;
        [SerializeField] TextMeshPro levelName;

        private MapLevelPanel mapLevelPanel;

        private void Awake()
        {
            mapLevelPanel = GetComponentInChildren<MapLevelPanel>();
        }

        private void Start()
        {
            levelName.text = level.levelName;
            mapLevelPanel.SetupPanel(level.description, level.GetTotalCoins().ToString(), level.rewardMob, level.rewardHero);
        }

        
        public string GetLevelID()
        {
            return level ? level.GetId() : null;
        }

        public bool IsUnlockedByDefault()
        {
            return level.IsUnlocked;
        }

        private void OnMouseDown()
        {
            if (!ProgressManager.Instance.CheckIfTeamIsFull()) return;
            
            SaveSystem.Instance.SaveGame();
            ProgressManager.Instance.LevelToLoad = level;
            FindAnyObjectByType<ScreenMap>().LoadLevel(level.scene.name);
        }

        private void OnMouseEnter()
        {
            mapLevelPanel.ShowPanel();
        }

        private void OnMouseExit()
        {
            mapLevelPanel.HidePanel();
        }
    }
}
