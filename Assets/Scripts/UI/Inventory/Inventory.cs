using System;
using System.Collections.Generic;
using UI.Inventory;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Transform itemPosition;
    [SerializeField] private Transform elementsPanel;
    [SerializeField] private Button cancelButton;

    private List<Item> items = new ();
    
    public Transform ItemPosition => itemPosition;
    
    private ItemDescription descriptionPanel;

    private void Start()
    {
        transform.localScale = Vector3.zero;
        
        descriptionPanel = GetComponentInChildren<ItemDescription>();
        descriptionPanel.gameObject.SetActive(false);
        items.AddRange(elementsPanel.GetComponentsInChildren<Item>());
        cancelButton.onClick.AddListener(Cancel);
        cancelButton.gameObject.SetActive(false);
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

    public void ResetItem(Item item)
    {
        cancelButton.gameObject.SetActive(true);
        
        foreach (var obj in items)
        {
            if (obj != item) obj.ActivateItem();
        }

        foreach (Transform child in itemPosition)
        {
            Destroy(child.gameObject);
        }
    }

    public void Cancel()
    {
        cancelButton.gameObject.SetActive(false);
        
        foreach (var obj in items)
        {
            obj.ActivateItem();
        }

        foreach (Transform child in itemPosition)
        {
            Destroy(child.gameObject);
        }
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
        Cancel();
    }
}
