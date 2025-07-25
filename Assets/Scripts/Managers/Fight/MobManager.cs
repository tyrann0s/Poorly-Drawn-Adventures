using System;
using System.Collections.Generic;
using System.Linq;
using Managers.Base;
using Mobs;
using UnityEngine;

namespace Managers
{
    public class MobManager : MonoBehaviour, IManager
    {
        public int CurrentWave { get; set; }
        public int MaxWaves { get; set; }

        private List<MobSpawner> playerMobSpawners = new();
        private List<MobSpawner> enemyMobSpawners = new();
        
        public List<Mob> PlayerMobs { get; set; } = new ();
        public List<Mob> EnemyMobs { get; set; } = new ();
        
        public void Initialize()
        {
            // Готовим спавнеры мобов
            MaxWaves = ServiceLocator.Get<GameManager>().CurrentLevel.mobWaves.Count;
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
            foreach (var currentMob in ServiceLocator.Get<GameManager>().CurrentLevel.mobWaves[CurrentWave].mobs)
            { 
                AddMob(enemyMobSpawners[currentSpawner].SpawnMob(currentMob.mobPrefab));
                ProgressManager.Instance.UnlockRecord(currentMob, false, false);
                currentSpawner++;
            }
        }

        public void SpawnBoss()
        {
            var bossData = ServiceLocator.Get<GameManager>().CurrentLevel.boss;
            ProgressManager.Instance.UnlockRecord(bossData, false, false);
            AddMob(enemyMobSpawners[3].SpawnMob(bossData.mobPrefab));
        }

        public void SpawnPlayerMobs()
        {
            int currentSpawner = 0;
            
            foreach (var currentMob in ProgressManager.Instance.CurrentTeam)
            {
                AddMob(playerMobSpawners[currentSpawner].SpawnMob(currentMob.mobPrefab));
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

            if (mob.MobData.Type == MobType.Boss)
            {
                ServiceLocator.Get<GameManager>().Win();
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
                    ProgressManager.Instance.CurrentTeam.Remove(mob.MobData);
                }
                else
                {
                    Debug.LogWarning($"Mob {mob.name} not found in player mobs list!");
                }
            }
        }
    }
}
