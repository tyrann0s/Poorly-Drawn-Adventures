using UnityEngine;

namespace Managers.Hub
{
    public class MapManager : MonoBehaviour
    {
        public static MapManager Instance { get; private set; }
        private static MapManager instance;

        private void Awake()
        {
            if (Instance && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }
    }
}
