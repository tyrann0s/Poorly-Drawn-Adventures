using Cards;
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

        //CreateRecord<RecordCombo>().Initialize(combo);
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
