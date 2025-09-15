using System;
using System.Threading.Tasks;
using Managers;
using Mobs;
using UnityEngine;

public class Bootstrap : EntryPoint
{
    [SerializeField] private MobData defaultMob;
    
    protected override async Task CheckDependencies()
    {
        Debug.Log("Начало инициализации");
        
        var resourceManager = gameObject.AddComponent<ResourceManager>();
        var saveSystem = gameObject.AddComponent<SaveSystem>();
        var progressManager = gameObject.AddComponent<ProgressManager>();
        progressManager.DefaultMob = defaultMob;

        await InitializeService(resourceManager);
        await InitializeService(saveSystem);
        await InitializeService(progressManager);
        
        Debug.Log("Все сервисы инициализированы");
        
        OnFinish();
    }

    protected override void OnFinish()
    {
        FindAnyObjectByType<LoadingScreen>(findObjectsInactive: FindObjectsInactive.Include).LoadBase();
    }
}
