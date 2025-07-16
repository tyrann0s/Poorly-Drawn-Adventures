using Cards;
using Levels;
using Managers;
using Mobs;
using UnityEngine;

public class FightEntryPoint : EntryPoint
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private MobManager mobManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private QueueManager queueManager;
    [SerializeField] private MusicManager musicManager;
    
    [SerializeField] private CardPanel cardPanel;
    
    [SerializeField] private MobData defaultMob;
    [SerializeField] private Level defaultLevel;
    
    
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
