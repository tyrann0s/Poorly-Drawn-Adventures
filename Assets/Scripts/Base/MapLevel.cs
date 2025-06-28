using System;
using Hub.UI;
using Levels;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            mapLevelPanel.SetupPanel(level.description, level.GetTotalCoins().ToString(), level.rewardMob.MobName, level.rewardMob.mobIcon);
        }

        private void OnMouseDown()
        {
            SaveSystem.Instance.SaveGame();
            SceneManager.LoadScene(level.scene.name);
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
