using System.Threading.Tasks;
using Hub;
using Managers;
using Managers.Base;
using Mobs;
using UnityEngine;

public class BaseEntryPoint : EntryPoint
{
    [SerializeField] private BaseManager baseManager;
    [SerializeField] private MobData defaultMob;
    [SerializeField] private GameObject levels;
    [SerializeField] private GameObject journalPrefab;
    
    protected override async Task CheckDependencies()
    {
        GameObject baseEntry = new GameObject("Base Entry");
        
        if (!ResourceManager.Instance)
        {
            var resourceManager = baseEntry.AddComponent<ResourceManager>();
            await InitializeService(resourceManager);       
        }
        
        if (!SaveSystem.Instance)
        {
            var saveSystem = baseEntry.AddComponent<SaveSystem>();
            await InitializeService(saveSystem);
        }
        
        if (!ProgressManager.Instance)
        {
            var progressManager = baseEntry.AddComponent<ProgressManager>();
            progressManager.DefaultMob = defaultMob;
            await InitializeService(progressManager);       
        }
        
        OnFinish();
    }

    protected override void OnFinish()
    {
        ServiceLocator.Register(baseManager);
        PrepareMap();
        Instantiate(journalPrefab);
        baseManager.Initialize();
    }

    private void PrepareMap()
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
