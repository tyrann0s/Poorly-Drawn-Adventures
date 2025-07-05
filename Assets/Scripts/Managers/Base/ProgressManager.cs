using System;
using System.Collections.Generic;
using Levels;
using Mobs;
using UnityEngine;

namespace Managers.Base
{
    public class ProgressManager : MonoBehaviour
    {
        public static ProgressManager Instance { get; private set; }

        [SerializeField] private MobData defaultMob;
        
        public int Coins { get; set; }
        public List<MobData> CurrentTeam { get; set; } = new();
        public List<MobData> AvailableHeroes { get; set; } = new();
        public List<MobData> AvailableMobs { get; set; } = new();
        
        public List<string> MapLevelsUnlocked { get; set; } = new();
        public Level LevelToLoad { get; set; }
    
        private void Awake()
        {
            if (Instance && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            DontDestroyOnLoad(this);
            Instance = this;
            
            SaveSystem.Instance.LoadGame();
            if (!AvailableMobs.Contains(defaultMob)) AvailableMobs.Add(defaultMob);
        }
        
        public void UnlockHero(MobData mobData)
        {
            AvailableHeroes.Add(mobData);
        }

        public void UnlockMob(MobData mobData)
        {
            AvailableMobs.Add(mobData);
        }
        
        public void UnlockLevel(string levelID)
        {
            MapLevelsUnlocked.Add(levelID);
        }
        
        public bool CheckIfTeamIsFull()
        {
            if (CurrentTeam.Count < 4)
            {
                Debug.Log("КОМАНДА ДОЛЖА БЫТЬ ПОЛНОЙ");
                return false;
            }
            
            return true;
        }
    }
}
