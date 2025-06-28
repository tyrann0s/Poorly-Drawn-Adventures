using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int coins;
    public List<string> currentTeam = new();
    public List<string> heroesUnlocked = new();
    public List<string> mobsUnlocked = new();
    public List<LevelSaveData> levelsCleared = new();
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
