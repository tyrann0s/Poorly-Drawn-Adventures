using System.Collections.Generic;
using Cards;
using Levels;
using Managers;
using Mobs;
using UnityEngine;

public class FightEntryPoint : EntryPoint
{
    [Header("Менеджеры")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private MobManager mobManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private QueueManager queueManager;
    [SerializeField] private MusicManager musicManager;
    [SerializeField] private CardPanel cardPanel;
    
    [Header("Параметры по-умолчанию")]
    [SerializeField] private MobData defaultMob;
    [SerializeField] private List<MobData> defaultTeam;
    [SerializeField] private Level defaultLevel;
    
    [Header("Журнал")]
    [SerializeField] private GameObject journalPrefab;
    
    protected override async void CheckDependencies()
    {
        GameObject fightEntry = new GameObject("Fight Entry");
        
        if (!ResourceManager.Instance)
        {
            var resourceManager = fightEntry.AddComponent<ResourceManager>();
            await InitializeService(resourceManager);       
        }
        
        if (!SaveSystem.Instance)
        {
            var saveSystem = fightEntry.AddComponent<SaveSystem>();
            await InitializeService(saveSystem);
        }
        
        if (!ProgressManager.Instance)
        {
            var progressManager = fightEntry.AddComponent<ProgressManager>();
            
            progressManager.DefaultMob = defaultMob;
            progressManager.CurrentTeam.AddRange(defaultTeam);
            progressManager.LevelToLoad = defaultLevel;
            
            await InitializeService(progressManager);       
        }
        
        OnFinish();
    }

    protected override void OnFinish()
    {
        ServiceLocator.Register(gameManager);
        ServiceLocator.Register(mobManager);
        ServiceLocator.Register(uiManager);
        ServiceLocator.Register(queueManager);
        ServiceLocator.Register(musicManager);
        ServiceLocator.Register(cardPanel);
        
        
        uiManager.Initialize();
        cardPanel.Initialize();
        gameManager.CurrentLevel = defaultLevel;
        gameManager.Initialize();
        mobManager.Initialize();

        Instantiate(journalPrefab);
    }
}
