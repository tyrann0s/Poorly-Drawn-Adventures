using Cards;
using Managers;
using Managers.Base;
using Mobs;
using UnityEngine;
using UnityEngine.UI;

public class RecordsPanel : MonoBehaviour
{
    [SerializeField] private GameObject recordPrefab;
    [SerializeField] private MobData mobData;
    
    public void ClearRecords()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    private Record CreateRecord()
    {
        GameObject go = Instantiate(recordPrefab, transform);
        return go.GetComponent<Record>();
    }
    
    public void ShowElementalCombos()
    {
        ClearRecords();

        foreach (var combo in ProgressManager.Instance.RecordsCombo)
        {
            if (combo.elementConditions.Count > 0 || combo.mixedConditions.Count > 0) CreateRecord().Initialize(combo);
        }
    }

    public void ShowRankCombos()
    {
        ClearRecords();
        
        foreach (var combo in ProgressManager.Instance.RecordsCombo)
        {
            if (combo.rankConditions.Count > 0) CreateRecord().Initialize(combo);
        }
    }

    public void ShowSpecialCombos()
    {
        ClearRecords();
        
        foreach (var combo in ProgressManager.Instance.RecordsCombo)
        {
            if (combo.specificConditions.Count > 0) CreateRecord().Initialize(combo);
        }
    }
    
    public void ShowEnemies()
    {
        ClearRecords();
        
        foreach (var mob in ProgressManager.Instance.RecordsMob)
        {
            var data = ResourceManager.Instance.GetMobData(mob.mobData);
            if (data.Type == MobType.Enemy) CreateRecord().Initialize(mob);
        }
    }

    public void ShowBosses()
    {
        ClearRecords();
        
        foreach (var mob in ProgressManager.Instance.RecordsMob)
        {
            var data = ResourceManager.Instance.GetMobData(mob.mobData);
            if (data.Type == MobType.Boss) CreateRecord().Initialize(mob);
        }
    }
    
    public void ShowAllies()
    {
        ClearRecords();
        foreach (var mob in ProgressManager.Instance.RecordsMob)
        {
            var data = ResourceManager.Instance.GetMobData(mob.mobData);
            if (data.Type == MobType.Ally) CreateRecord().Initialize(mob);
        }
    }

    public void ShowHeroes()
    {
        ClearRecords();
        
        foreach (var mob in ProgressManager.Instance.RecordsMob)
        {
            var data = ResourceManager.Instance.GetMobData(mob.mobData);
            if (data.Type == MobType.Hero) CreateRecord().Initialize(mob);
        }
    }
}
