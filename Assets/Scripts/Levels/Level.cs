using System.Collections.Generic;
using Mobs;
using UnityEditor;
using UnityEngine;

namespace Levels
{
    [CreateAssetMenu (fileName = "Level", menuName = "Data/Levels/Level", order = 0)]
    public class Level : ScriptableObject
    {
        public bool IsUnlocked;
        public SceneAsset scene;
        public string levelName;
        public List<MobWave> mobWaves = new();
        public MobData boss;
        public int coinsForWave = 100;
        [TextArea(3, 10)]
        public string description;

        public MobData rewardMob;
        public MobData rewardHero;

        [SerializeField]
        private Level nextLevel;
        
        public int GetTotalCoins()
        {
            return mobWaves.Count * coinsForWave;;
        }
        
        public string GetId()
        {
            return name;
        }
        
        public string GetNextLevel()
        {
            if (nextLevel) return nextLevel.GetId();
            return null;
        }
    }
}
