using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int coins;
    public List<MobSaveData> currentTeam = new();
    public List<MobSaveData> heroesUnlocked = new();
    public List<MobSaveData> mobsUnlocked = new();
    public List<LevelSaveData> levelsCleared = new();
}

[System.Serializable]
public class MobSaveData
{
    public string mobID;

    public MobSaveData(string id)
    {
        mobID = id;
    }
}

[System.Serializable]
public class LevelSaveData
{
    public int levelID;

    public LevelSaveData(int id)
    {
        levelID = id;
    }
}
