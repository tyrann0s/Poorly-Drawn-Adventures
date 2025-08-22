using TMPro;
using UnityEngine;

public class MobInfoPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI vulnerabiltyText, immunityText;
    
    public void SetVulnerability(string value)
    {
        vulnerabiltyText.text = value;
    }
    
    public void SetImmunity(string value)
    {
        immunityText.text = value;
    }
}
