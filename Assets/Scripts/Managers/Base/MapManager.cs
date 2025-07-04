using System;
using System.Collections.Generic;
using Hub;
using Managers.Base;
using UnityEngine;

namespace Managers.Hub
{
    public class MapManager : MonoBehaviour
    {
        public static MapManager Instance { get; private set; }

        [SerializeField] private GameObject levels;

        private void Awake()
        {
            if (Instance && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            foreach (Transform child in levels.transform)
            {
                child.gameObject.SetActive(false);
                
                var mapLevel = child.GetComponent<MapLevel>();
                
                if (mapLevel)
                {
                    if (mapLevel.IsUnlockedByDefault() || ProgressManager.Instance.MapLevelsUnlocked.Contains(mapLevel.GetLevelID()))
                    {
                        mapLevel.gameObject.SetActive(true);   
                    }
                }
            }
        }
    }
}
