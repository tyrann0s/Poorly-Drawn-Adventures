using System;
using System.Collections.Generic;
using System.Linq;
using Mobs;
using UnityEngine;

namespace Managers
{
    public class MobManager : MonoBehaviour
    {
        private static MobManager instance;
        public static MobManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindFirstObjectByType<MobManager>();
                    if (instance == null)
                    {
                        GameObject go = new GameObject("Mob Manager");
                        instance = go.AddComponent<MobManager>();
                    }
                }
                return instance;
            }
        }
        
        public int CurrentWave { get; set; }
        public int MaxWaves { get; set; }

        private List<MobSpawner> playerMobSpawners = new();
        private List<MobSpawner> enemyMobSpawners = new();
        
        public List<Mob> PlayerMobs { get; set; } = new ();
        public List<Mob> EnemyMobs { get; set; } = new ();
        
        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
        }

        private void Start()
        {
            // Готовим спавнеры мобов
            MaxWaves = GameManager.Instance.CurrentLevel.mobWaves.Count;
            foreach (var mobSpawner in FindObjectsByType<MobSpawner>(sortMode: FindObjectsSortMode.None)
                         .OrderBy(spawner => spawner.name)
                         .ToList())
            {
                if (mobSpawner.IsHostile) enemyMobSpawners.Add(mobSpawner);
                else playerMobSpawners.Add(mobSpawner);
            }
            
            SpawnPlayerMobs();
            SpawnNextWave();
        }
        
        public void SpawnNextWave()
        {
            int currentSpawner = 0;
            foreach (var currentMob in GameManager.Instance.CurrentLevel.mobWaves[CurrentWave].mobPrefabs)
            {
                AddMob(enemyMobSpawners[currentSpawner].SpawnMob(currentMob, false));
                currentSpawner++;
            }
        }

        public void SpawnBoss()
        {
            AddMob(enemyMobSpawners[3].SpawnMob(GameManager.Instance.CurrentLevel.bossPrefab, true));
        }

        public void SpawnPlayerMobs()
        {
            int currentSpawner = 0;
            foreach (var currentMob in PlayerManager.Instance.MobPrefabs)
            {
                AddMob(playerMobSpawners[currentSpawner].SpawnMob(currentMob, false));
                currentSpawner++;
            }
        }

        private void AddMob(Mob mob)
        {
            if (!mob)
            {
                Debug.LogError("Trying to add null mob!");
                return;
            }

            if (mob.IsHostile)
            {
                if (!EnemyMobs.Contains(mob))
                {
                    EnemyMobs.Add(mob);
                }
                else
                {
                    Debug.LogWarning($"Mob {mob.name} is already in enemy mobs list!");
                }
            }
            else
            {
                if (!PlayerMobs.Contains(mob))
                {
                    PlayerMobs.Add(mob);
                }
                else
                {
                    Debug.LogWarning($"Mob {mob.name} is already in player mobs list!");
                }
            }
        }

        public void MobDied(Mob mob)
        {
            if (!mob)
            {
                Debug.LogError("Trying to remove null mob!");
                return;
            }

            if (mob.IsBoss)
            {
                GameManager.Instance.Win();
                return;
            }

            if (mob.State == MobState.Activated)
            {
                mob.State = MobState.Dead;
            }

            if (mob.IsHostile)
            {
                if (EnemyMobs.Contains(mob))
                {
                    EnemyMobs.Remove(mob);
                }
                else
                {
                    Debug.LogWarning($"Mob {mob.name} not found in enemy mobs list!");
                }
            }
            else
            {
                if (PlayerMobs.Contains(mob))
                {
                    PlayerMobs.Remove(mob);
                }
                else
                {
                    Debug.LogWarning($"Mob {mob.name} not found in player mobs list!");
                }
            }

            GameManager.Instance.CheckWinCondition();
        }
    }
}
