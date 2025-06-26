using UnityEngine;

namespace Managers.Hub
{
    public class HubManager : MonoBehaviour
    {
        public static HubManager Instance { get; private set; }
        private static HubManager instance;

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
