using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class JournalPageButton : MonoBehaviour
{
    private TextMeshProUGUI buttonText;
    private Button button;

    private void Awake()
    {
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();
    }

    public void Initialize(string text, UnityAction onClick)
    {
        buttonText.text = text;
        button.onClick.AddListener(onClick);
    }
}
