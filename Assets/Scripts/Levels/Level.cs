using System.Collections.Generic;
using Mobs;
using UnityEngine;

namespace Levels
{
    [CreateAssetMenu (fileName = "Level", menuName = "Data/Levels/Level", order = 0)]
    public class Level : ScriptableObject
    {
        public List<MobWave> mobWaves = new();
        public GameObject bossPrefab;
        public float coinsForWave = 100f;
    }
}
