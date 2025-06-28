using System.Collections.Generic;
using Mobs;
using UnityEngine;

namespace Levels
{
    [CreateAssetMenu (fileName = "Mob Wave", menuName = "Data/Levels/Mob Wave", order = 1)]
    public class MobWave : ScriptableObject
    {
        public List<MobData> mobs= new(4);
    }
}
