using System;
using System.Collections.Generic;
using Mobs;
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
public class MobRecord : IComparable<MobRecord>
{
    public string mobData;
    public bool unlockVulnerabilty;
    public bool unlockImmune;

    public MobRecord(string mobData, bool unlockVulnerabilty, bool unlockImmune)
    {
        this.mobData = mobData;
        this.unlockVulnerabilty = unlockVulnerabilty;
        this.unlockImmune = unlockImmune;
    }

    public int CompareTo(MobRecord other)
    {
        if (other == null) return 1;
        return string.Compare(mobData, other.mobData, StringComparison.OrdinalIgnoreCase);

    }
}
