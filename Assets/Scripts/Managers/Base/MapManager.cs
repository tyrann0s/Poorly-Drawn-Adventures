using Hub;
using UnityEngine;

namespace Managers.Hub
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField] private GameObject levels;

        private void Start()
        {
            foreach (Transform child in levels.transform)
            {
                child.gameObject.SetActive(false);
                
                var mapLevel = child.GetComponent<MapLevel>();
                
                if (mapLevel)
                {
                    if (mapLevel.IsUnlockedByDefault() || ProgressManager.Instance.MapLevelsUnlocked.Contains(mapLevel.GetLevelID()))
                    {
                        mapLevel.gameObject.SetActive(true);   
                    }
                }
            }
        }
    }
}
