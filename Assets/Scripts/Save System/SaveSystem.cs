using System.Collections.Generic;
using System.IO;
using Cards;
using Managers.Base;
using Mobs;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance { get; private set; }
    
    private SaveData saveData = new();
    private string savePath;
    
    private Dictionary<string, MobData> mobLookup;
    private Dictionary<string, ElementCombo> comboLookup;
    
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
        CreateComboDB();
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
    
    private void CreateComboDB()
    {
        ElementCombo[] allCombos = Resources.LoadAll<ElementCombo>("Data/Card Combinations");
        
        comboLookup = new Dictionary<string, ElementCombo>();
        
        foreach (var combo in allCombos)
        {
            comboLookup[combo.GetId()] = combo;
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
        foreach (var mobData in ProgressManager.Instance.AvailableAllies)
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
        
        // Сохраняем открытые комбо в журнале
        foreach (var record in ProgressManager.Instance.RecordsCombo)
        {
            if (!saveData.recordsCombo.Contains(record.GetId()))
            {
                saveData.recordsCombo.Add(record.GetId());
            }
        }

        // Сохраняем открытых мобов в журнале
        foreach (var record in ProgressManager.Instance.RecordsMob)
        {
            if (!saveData.recordsMobs.Contains(record))
            {
                saveData.recordsMobs.Add(record);   
            }
            else
            {
                int index = saveData.recordsMobs.FindIndex(x => x.name == record.name);
                saveData.recordsMobs[index] = record;  
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
                ProgressManager.Instance.AvailableAllies.Add(mobSO);
            } else Debug.LogError("Не удалось найти мобов");
        }
        
        // Загружаем пройденные уровни
        foreach (var level in saveData.levelsCleared)
        {
            ProgressManager.Instance.MapLevelsUnlocked.Add(level);
        }
        
        // Загружаем открытые комбо в журнале
        foreach (var combo in saveData.recordsCombo)
        {
            if (comboLookup.TryGetValue(combo, out ElementCombo comboSO))
            {
                ProgressManager.Instance.RecordsCombo.Add(comboSO);
            } else Debug.LogError("Не удалось найти комбо");
        }
        
        // Загружаем открытых мобов в журнале
        foreach (var mob in saveData.recordsMobs)
        {
            if (mobLookup.TryGetValue(mob.name, out MobData mobSO))
            {
                ProgressManager.Instance.RecordsMob.Add(mob);
            } else Debug.LogError("Не удалось найти запись с мобом");
        }
    }
}