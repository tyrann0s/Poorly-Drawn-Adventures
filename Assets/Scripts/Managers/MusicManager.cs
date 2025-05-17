using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource calm, battle, win, lose;
    public AudioSource Calm => calm;
    public AudioSource Battle => battle;
    public AudioSource Win => win;
    public AudioSource Lose => lose;

    private float calmVolume, battleVolume, winVolume, loseVolume;

    [SerializeField]
    private float fadeTime;

    private void Awake()
    {
        calmVolume = calm.volume;
        battleVolume = battle.volume;
        winVolume = win.volume;
        loseVolume = lose.volume;
    }

    public void StartCalmMusic()
    {
        calm.volume = calmVolume;
        calm.Play();
    }

    public void StartBattleMusic()
    {
        battle.volume = battleVolume;
        battle.Play();
    }

    public void StartWinMusic()
    {
        win.volume = winVolume;
        win.Play();
    }

    public void StartLoseMusic()
    {
        lose.volume = loseVolume;
        lose.Play();
    }

    public IEnumerator FadeTrackToZero(AudioSource clip)
    {
        float t = 0f;
        float blend;
        float originalVolume = clip.volume;
        float targetVolume;

        while (t < fadeTime)
        {
            t += Time.deltaTime;

            blend = Mathf.Clamp01(t / fadeTime);
            targetVolume = Mathf.Lerp(originalVolume, 0, blend);

            clip.volume = targetVolume;
            yield return null;
        }

        clip.Stop();
    }
}
