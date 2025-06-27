using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public float coins;
    public List<MobSaveData> heroesUnlocked = new();
    public List<MobSaveData> mobsUnlocked = new();
    public List<LevelSaveData> levelsCleared = new();
}

[System.Serializable]
public class MobSaveData
{
    public int mobID;

    public MobSaveData(int id)
    {
        mobID = mobID;
    }
}

[System.Serializable]
public class LevelSaveData
{
    public int levelID;

    public LevelSaveData(int id)
    {
        levelID = levelID;
    }
}
