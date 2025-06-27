using System.Collections.Generic;
using System.IO;
using Managers.Base;
using Mobs;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance { get; private set; }
    
    private SaveData saveData = new();
    private string savePath;
    
    private Dictionary<string, MobData> mobLookup;
    
    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
        
        savePath = Path.Combine(Application.dataPath + "/Save", "save.json");
        CreateMobDB();
    }
    
    private void CreateMobDB()
    {
        MobData[] allMobs = Resources.LoadAll<MobData>("Data/Mobs Data");
        
        mobLookup = new Dictionary<string, MobData>();
        
        foreach (var mob in allMobs)
        {
            mobLookup[mob.GetId()] = mob;
        }
        
        Debug.Log($"Загружено мобов в базу данных: {allMobs.Length}");
    }
    
    public void SaveGame()
    {
        CollectSaveData();

        var json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(savePath, json);
    
        Debug.Log("Сохранено в: " + savePath);
    }

    private void CollectSaveData()
    {
        saveData = new SaveData();
        
        saveData.coins = BaseManager.Instance.Coins;
        
        // Сохраняем текущую команду
        foreach (var mobData in BaseManager.Instance.CurrentTeam)
        {
            saveData.currentTeam.Add(new MobSaveData(mobData.GetId()));
        }
        
        // Сохраняем героев
        foreach (var mobData in BaseManager.Instance.AvailableHeroes)
        {
            saveData.heroesUnlocked.Add(new MobSaveData(mobData.GetId()));
        }
        
        // Сохраняем обычных мобов
        foreach (var mobData in BaseManager.Instance.AvailableMobs)
        {
            saveData.mobsUnlocked.Add(new MobSaveData(mobData.GetId()));
        }
    }

    public void LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            saveData = JsonUtility.FromJson<SaveData>(json);
            ApplyData();
        }
    }

    private void ApplyData()
    {
        BaseManager.Instance.Coins = saveData.coins;
        
        // Загружаем текущую команду
        foreach (var mobSaveData in saveData.currentTeam)
        {
            if (mobLookup.TryGetValue(mobSaveData.mobID, out MobData mobSO))
            {
                BaseManager.Instance.CurrentTeam.Add(mobSO);
            } else Debug.LogError("ШО ЗА ХУЙНЯ");
        }
        
        // Загружаем героев
        foreach (var mobSaveData in saveData.heroesUnlocked)
        {
            if (mobLookup.TryGetValue(mobSaveData.mobID, out MobData mobSO))
            {
                BaseManager.Instance.AvailableHeroes.Add(mobSO);
            } else Debug.LogError("ШО ЗА ХУЙНЯ");
        }
        
        // Загружаем мобов
        foreach (var mobSaveData in saveData.mobsUnlocked)
        {
            if (mobLookup.TryGetValue(mobSaveData.mobID, out MobData mobSO))
            {
                BaseManager.Instance.AvailableMobs.Add(mobSO);
            } else Debug.LogError("ШО ЗА ХУЙНЯ");
        }
    }
}
