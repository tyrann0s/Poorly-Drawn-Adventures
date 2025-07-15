using Cards;
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
        
        foreach (var comboombo in ProgressManager.Instance.RecordsCombo)
        {
            if (comboombo.rankConditions.Count > 0) CreateRecord().Initialize(comboombo);
        }
    }

    public void ShowSpecialCombos()
    {
        ClearRecords();
        
        foreach (var comboombo in ProgressManager.Instance.RecordsCombo)
        {
            if (comboombo.specificConditions.Count > 0) CreateRecord().Initialize(comboombo);
        }
    }
    
    public void ShowEnemies()
    {
        ClearRecords();
        
        CreateRecord().Initialize(mobData);
    }

    public void ShowBosses()
    {
        ClearRecords();
        
        Debug.Log("ShowBosses");
    }
    
    public void ShowAllies()
    {
        ClearRecords();
        
        Debug.Log("ShowAllies");
    }

    public void ShowHeroes()
    {
        ClearRecords();
        
        Debug.Log("ShowHeroes");
    }
}
