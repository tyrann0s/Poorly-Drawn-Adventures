using System;
using System.Collections.Generic;
using Cards;
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
        public List<MobData> AvailableAllies { get; set; } = new();
        
        public List<string> MapLevelsUnlocked { get; set; } = new();
        public Level LevelToLoad { get; set; }
        
        public List<ElementCombo> RecordsCombo { get; set; } = new();
        public List<MobRecord> RecordsMob { get; set; } = new();
    
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
            if (!AvailableAllies.Contains(defaultMob)) AvailableAllies.Add(defaultMob);
        }
        
        public void UnlockHero(MobData mobData)
        {
            AvailableHeroes.Add(mobData);
        }

        public void UnlockMob(MobData mobData)
        {
            AvailableAllies.Add(mobData);
        }
        
        public void UnlockLevel(string levelID)
        {
            MapLevelsUnlocked.Add(levelID);
        }

        public void UnlockRecord(ElementCombo combo)
        {
            RecordsCombo.Add(combo);
            Debug.Log($"{combo} unlocked!");
        }

        public void UnlockRecord(MobData mobData)
        {
            RecordsMob.Add(new MobRecord(mobData.name, mobData.VulnerableTo, mobData.ImmuneTo));
        }
    }
}
