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
        
        saveData.coins = ProgressManager.Instance.Coins;
        
        // Сохраняем текущую команду
        foreach (var mobData in ProgressManager.Instance.CurrentTeam)
        {
            saveData.currentTeam.Add(mobData.GetId());
        }
        
        // Сохраняем героев
        foreach (var mobData in ProgressManager.Instance.AvailableHeroes)
        {
            if (!saveData.heroesUnlocked.Contains(mobData.GetId()))
            {
                saveData.heroesUnlocked.Add(mobData.GetId());
            }
        }
        
        // Сохраняем обычных мобов
        foreach (var mobData in ProgressManager.Instance.AvailableMobs)
        {
            if (!saveData.mobsUnlocked.Contains(mobData.GetId()))
            {
                saveData.mobsUnlocked.Add(mobData.GetId());
            }
        }
        
        // Сохраняем прогресс пройденных уровней
        foreach (var level in ProgressManager.Instance.MapLevelsUnlocked)
        {
            if (!saveData.levelsCleared.Contains(level))
            {
                saveData.levelsCleared.Add(level);
            }
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
        ProgressManager.Instance.Coins = saveData.coins;
        
        // Загружаем текущую команду
        foreach (var mobSaveData in saveData.currentTeam)
        {
            if (mobLookup.TryGetValue(mobSaveData, out MobData mobSO))
            {
                ProgressManager.Instance.CurrentTeam.Add(mobSO);
            } else Debug.LogError("Не удалось найти мобов текущей команды");
        }
        
        // Загружаем героев
        foreach (var mobSaveData in saveData.heroesUnlocked)
        {
            if (mobLookup.TryGetValue(mobSaveData, out MobData mobSO))
            {
                ProgressManager.Instance.AvailableHeroes.Add(mobSO);
            } else Debug.LogError("Не удалось найти героев");
        }
        
        // Загружаем мобов
        foreach (var mobSaveData in saveData.mobsUnlocked)
        {
            if (mobLookup.TryGetValue(mobSaveData, out MobData mobSO))
            {
                ProgressManager.Instance.AvailableMobs.Add(mobSO);
            } else Debug.LogError("Не удалось найти мобов");
        }
        
        // Загружаем пройденные уровни
        foreach (var level in saveData.levelsCleared)
        {
            ProgressManager.Instance.MapLevelsUnlocked.Add(level);
        }
    }
}