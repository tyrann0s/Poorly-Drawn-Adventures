using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cards;
using Levels;
using Mobs;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Managers
{
    public class ProgressManager : MonoBehaviour, IService
    {
        public static ProgressManager Instance { get; private set; }

        public MobData DefaultMob { get; set; }
        
        public int Coins { get; set; }
        public List<MobData> CurrentTeam { get; set; } = new();
        public List<MobData> AvailableHeroes { get; set; } = new();
        public List<MobData> AvailableAllies { get; set; } = new();
        
        public List<string> MapLevelsUnlocked { get; set; } = new();
        public Level LevelToLoad { get; set; }
        
        public List<ElementCombo> RecordsCombo { get; set; } = new();
        public List<MobRecord> RecordsMob { get; set; } = new();
        
        public event Action<string> OnRecordChanged;
    
        private void Awake()
        {
            if (Instance && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        public Task InitializeAsync()
        {
            SaveSystem.Instance.LoadGame();
            if (!AvailableAllies.Contains(DefaultMob)) UnlockAlly(DefaultMob);
            return Task.CompletedTask;
        }
        
        public void UnlockHero(MobData mobData)
        {
            if (AvailableHeroes.Contains(mobData)) return;
            AvailableHeroes.Add(mobData);
            UnlockRecord(mobData, true, true);
        }

        public void UnlockAlly(MobData mobData)
        {
            if (AvailableAllies.Contains(mobData)) return;
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
                RecordsCombo.Sort();
                OnRecordChanged?.Invoke($"{combo.name} unlocked!");
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
                        OnRecordChanged?.Invoke($"{mobData.MobName} unlocked!");
                    }
                    else
                    {
                        int index = RecordsMob.FindIndex(x => x.mobData == record.mobData);
                        if (!RecordsMob[index].unlockImmune && immuneTo)
                        {
                            RecordsMob[index].unlockImmune = immuneTo;
                            OnRecordChanged?.Invoke($"{mobData.MobName} immunity discovered!");
                        }

                        if (!RecordsMob[index].unlockVulnerabilty && vulnerableTo)
                        {
                            RecordsMob[index].unlockVulnerabilty = vulnerableTo;
                            OnRecordChanged?.Invoke($"{mobData.MobName} vulnerability discovered!");
                        }
                    }
                    break;
                case MobType.Ally:
                case MobType.Hero:
                    if (RecordsMob.Find(x=>x.mobData == record.mobData) == null)
                    {
                        RecordsMob.Add(record);
                        OnRecordChanged?.Invoke($"{mobData.MobName} unlocked!");
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            RecordsMob.Sort();
        }
    }
}
