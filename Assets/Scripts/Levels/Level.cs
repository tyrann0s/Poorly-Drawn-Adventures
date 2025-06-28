using System.Collections.Generic;
using Mobs;
using UnityEditor;
using UnityEngine;

namespace Levels
{
    [CreateAssetMenu (fileName = "Level", menuName = "Data/Levels/Level", order = 0)]
    public class Level : ScriptableObject
    {
        public SceneAsset scene;
        public string levelName;
        public List<MobWave> mobWaves = new();
        public MobData boss;
        public float coinsForWave = 100f;
        [TextArea(3, 10)]
        public string description;

        public MobData rewardMob;
        
        public float GetTotalCoins()
        {
            return mobWaves.Count * coinsForWave;;
        }
    }
}
