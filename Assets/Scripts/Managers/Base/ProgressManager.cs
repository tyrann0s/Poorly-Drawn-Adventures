using System;
using Mobs;
using UnityEngine;

namespace Managers.Base
{
    public class ProgressManager : MonoBehaviour
    {
        public static ProgressManager Instance { get; private set; }
    
        private void Awake()
        {
            if (Instance && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            SaveSystem.Instance.SaveGame();
        }
    }
}
