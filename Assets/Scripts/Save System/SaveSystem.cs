using System;
using System.Collections.Generic;
using System.IO;
using Cards;
using Managers;
using Managers.Base;
using Mobs;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance { get; private set; }
    
    private SaveData saveData = new();
    private string savePath;
    
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
                int index = saveData.recordsMobs.FindIndex(x => x.mobData == record.mobData);
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
            try
            {
                ProgressManager.Instance.CurrentTeam.Add(ResourceManager.Instance.GetMobData(mobSaveData));
            }
            catch (Exception e)
            {
                Debug.LogError("Не удалось найти мобов текущей команды");
                throw;
            }
        }
        
        // Загружаем героев
        foreach (var mobSaveData in saveData.heroesUnlocked)
        {
            try
            {
                ProgressManager.Instance.AvailableHeroes.Add(ResourceManager.Instance.GetMobData(mobSaveData));
            }
            catch (Exception e)
            {
                Debug.LogError("Не удалось найти героя");
                throw;
            }
        }
        
        // Загружаем мобов
        foreach (var mobSaveData in saveData.mobsUnlocked)
        {
            try
            {
                ProgressManager.Instance.AvailableAllies.Add(ResourceManager.Instance.GetMobData(mobSaveData));
            }
            catch (Exception e)
            {
                Debug.LogError("Не удалось найти мобов");
                throw;
            }
        }
        
        // Загружаем пройденные уровни
        foreach (var level in saveData.levelsCleared)
        {
            ProgressManager.Instance.MapLevelsUnlocked.Add(level);
        }
        
        // Загружаем открытые комбо в журнале
        foreach (var combo in saveData.recordsCombo)
        {
            try
            {
                ProgressManager.Instance.RecordsCombo.Add(ResourceManager.Instance.GetComboData(combo));
            }
            catch (Exception e)
            {
                Debug.LogError("Не удалось найти комбо");
                throw;
            }
        }
        
        // Загружаем открытых мобов в журнале
        foreach (var mob in saveData.recordsMobs)
        {
            ProgressManager.Instance.RecordsMob.Add(mob);
        }
    }
}