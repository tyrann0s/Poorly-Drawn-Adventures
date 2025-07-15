using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int coins;
    public List<string> currentTeam = new();
    public List<string> heroesUnlocked = new();
    public List<string> mobsUnlocked = new();
    public List<string> levelsCleared = new();
    public List<string> recordsCombo = new();
    public List<MobRecord> recordsMobs = new();
}

[System.Serializable]
public class MobRecord
{
    public string name;
    public ElementType vulnerabiltyType;
    public ElementType immuneType;
    
    public MobRecord(string name, ElementType vulnerabiltyType, ElementType immuneType)
    {
        this.name = name;
        this.vulnerabiltyType = vulnerabiltyType;
        this.immuneType = immuneType;
    }
}
