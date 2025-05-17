using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    private Slider slider;
    public int MaxValue { get; set; }

    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();
    }

    public void Init(int maxValue)
    {
        slider.maxValue = maxValue;
        slider.value = maxValue;
    }

    public void UpdateHPBar(int value)
    {
        slider.value = value;
    }
}
