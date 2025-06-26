using System;
using System.Collections.Generic;
using Mobs;
using UnityEngine;

namespace Managers
{
    public class PlayerManager : MonoBehaviour
    {
        private static PlayerManager instance;
        public static PlayerManager Instance
        {
            get
            {
                if (!instance)
                {
                    instance = FindFirstObjectByType<PlayerManager>();
                    if (!instance)
                    {
                        GameObject go = new GameObject("PlayerManager");
                        instance = go.AddComponent<PlayerManager>();
                    }
                }
                return instance;
            }
        }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            
            MobPrefabs = new (testPrefabs);
        }

        public List<GameObject> MobPrefabs { get; private set; } = new();

        public List<GameObject> testPrefabs;
    }
}
