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
            if (!AvailableAllies.Contains(defaultMob)) UnlockAlly(defaultMob);
        }
        
        public void UnlockHero(MobData mobData)
        {
            AvailableHeroes.Add(mobData);
            UnlockRecord(mobData, true, true);
        }

        public void UnlockAlly(MobData mobData)
        {
            AvailableAllies.Add(mobData);
            UnlockRecord(mobData, true, true);
        }
        
        public void UnlockLevel(string levelID)
        {
            MapLevelsUnlocked.Add(levelID);
        }

        public void UnlockRecord(ElementCombo combo)
        {
            if (!Instance.RecordsCombo.Contains(combo))
            {
                RecordsCombo.Add(combo);
                Debug.Log($"{combo} unlocked!");
            }
        }

        public void UnlockRecord(MobData mobData, bool vulnerableTo, bool immuneTo)
        {
            var record = new MobRecord(mobData.name, vulnerableTo, immuneTo);
            
            switch (mobData.Type)
            {
                case MobType.Enemy:
                case MobType.Boss:
                    if (RecordsMob.Find(x=>x.mobData == record.mobData) == null)
                    {
                        RecordsMob.Add(record);
                        Debug.Log($"{mobData.MobName} unlocked!");
                    }
                    else
                    {
                        int index = RecordsMob.FindIndex(x => x.mobData == record.mobData);
                        RecordsMob[index] = record;  
                        Debug.Log($"{mobData.MobName} updated!");
                    }
                    break;
                case MobType.Ally:
                case MobType.Hero:
                    if (!RecordsMob.Contains(record))
                    {
                        RecordsMob.Add(record);
                        Debug.Log($"{mobData.MobName} unlocked!");
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
