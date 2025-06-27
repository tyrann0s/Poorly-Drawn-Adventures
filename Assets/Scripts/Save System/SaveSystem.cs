using System.IO;
using Managers.Base;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance { get; private set; }
    
    private SaveData saveData = new SaveData();
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
        LoadGame();
    }
    
    public void SaveGame()
    {
        CollectSaveData();
        
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(savePath, json);
        
        Debug.Log("Saved at " + savePath);
    }

    private void CollectSaveData()
    {
        saveData.coins = 666;
        foreach (var mobData in BaseManager.Instance.CurrentTeam)
        {
            Debug.Log(mobData.GetInstanceID());
            saveData.heroesUnlocked.Add(new MobSaveData(mobData.GetInstanceID()));
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
        
    }
}
