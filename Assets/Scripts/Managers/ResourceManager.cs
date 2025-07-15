using UnityEngine;

namespace Managers
{
    public class ResourceManager : MonoBehaviour
    {
        private static ResourceManager instance;
        public static ResourceManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<ResourceManager>();
                    if (instance == null)
                    {
                        GameObject go = new GameObject("ResourceManager");
                        instance = go.AddComponent<ResourceManager>();
                    }
                }
                return instance;
            }
        }

        public IconData Icons;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
            
            Icons = Resources.Load<IconData>("Settings/Icons Data");
        }
    }
}
