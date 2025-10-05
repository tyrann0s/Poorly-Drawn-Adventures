using System;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private ItemDescription descriptionPanel;

    private void Start()
    {
        descriptionPanel = GetComponentInChildren<ItemDescription>();
        descriptionPanel.gameObject.SetActive(false);
    }

    public void ShowDescription(string description)
    {
        descriptionPanel.gameObject.SetActive(true);
        descriptionPanel.SetDescription(description);
    }
    
    public void HideDescription()
    {
        descriptionPanel.gameObject.SetActive(false);
    }
}
