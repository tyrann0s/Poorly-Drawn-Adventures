using TMPro;
using UnityEngine;

public class ItemDescription : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI descriptionText;

    public void SetDescription(string text)
    {
        descriptionText.text = text;
    }
}
