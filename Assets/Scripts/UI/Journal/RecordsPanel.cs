using Cards;
using UnityEngine;
using UnityEngine.UI;

public class RecordsPanel : MonoBehaviour
{
    [SerializeField] private GameObject comboRecordPrefab, mobRecordPrefab;
    [SerializeField] private ElementCombo combo;
    
    private void ClearRecords()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
    
    public void ShowElementalCombos()
    {
        ClearRecords();
        
        GameObject go = Instantiate(comboRecordPrefab, transform);
        var record = go.GetComponent<RecordCombo>();
        record.Initialize(combo);
    }

    public void ShowRankCombos()
    {
        ClearRecords();
        
        Debug.Log("ShowRankCombos");
    }

    public void ShowSpecialCombos()
    {
        ClearRecords();
        
        Debug.Log("ShowSpecialCombos");
    }
    
    public void ShowEnemies()
    {
        ClearRecords();
        
        Debug.Log("ShowEnemies");
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
