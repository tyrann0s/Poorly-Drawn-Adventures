using System;
using System.Collections.Generic;
using Mobs;
using UnityEngine;

namespace Managers.Base
{
    public class ProgressManager : MonoBehaviour
    {
        public static ProgressManager Instance { get; private set; }

        [SerializeField] private MobData defaultMob;
        
        public List<MobData> CurrentTeam { get; set; } = new();
        public List<MobData> AvailableHeroes { get; set; } = new();
        public List<MobData> AvailableMobs { get; set; } = new();
    
        private void Awake()
        {
            if (Instance && Instance != this)
            {
                Destroy(gameObject);
                DontDestroyOnLoad(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            AvailableMobs.Add(defaultMob);
            SaveSystem.Instance.LoadGame();
        }
        
        public void UnlockHero(MobData mobData)
        {
            AvailableHeroes.Remove(mobData);
        }

        public void UnlockMob(MobData mobData)
        {
            AvailableMobs.Remove(mobData);
        }
    }
}
