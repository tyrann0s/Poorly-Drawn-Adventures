using System;
using Mobs;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MobButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private MBDescription descriptionPanel;
    
    public string DescriptionText { get; set; }

    private TextMeshProUGUI textTMP;
    private Button button; 

    private void Awake()
    {
        textTMP = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!button.interactable) return;
        
        descriptionPanel.ShowDescription(DescriptionText);
        descriptionPanel.transform.localPosition = new Vector3(descriptionPanel.transform.localPosition.x, 
            transform.localPosition.y + 75, 0);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        descriptionPanel.HideDescription();
    }

    public void SetButtonText(string text)
    {
        textTMP.text = text;
    }
    
    public void SetActive(bool value)
    {
        button.interactable = value;
    }

    public void Enable()
    {
        button.interactable = true;
    }

    public void Disable()
    {
        button.interactable = false;
    }
}