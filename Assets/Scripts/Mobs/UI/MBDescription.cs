using TMPro;
using UnityEngine;

public class MBDescription : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI descriptionText;
    
    public void ShowDescription(string value)
    {
        gameObject.SetActive(true);
        descriptionText.text = value;
    }
    
    public void HideDescription()
    {
        gameObject.SetActive(false);
        descriptionText.text = "";
    }
}
