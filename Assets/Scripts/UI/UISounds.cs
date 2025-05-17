using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISounds : MonoBehaviour
{
    [SerializeField]
    private AudioSource buttonHover, buttonClick, actionConfirm, shieldActivation, winSound, defeatSound, startBattle, endBattle;

    public void ButtonHover()
    {
        buttonHover.Play();
    }

    public void ButtonClick()
    {
        buttonClick.Play();
    }

    public void ActionConfirm()
    {
        actionConfirm.Play();
    }

    public void ShieldActivation()
    {
        shieldActivation.Play();
    }

    public void WinSound()
    {
        winSound.Play();
    }

    public void DefeatSound()
    {
        defeatSound.Play();
    }

    public void StartBattle()
    {
        startBattle.Play();
    }

    public void Endattle()
    {
        endBattle.Play();
    }
}
