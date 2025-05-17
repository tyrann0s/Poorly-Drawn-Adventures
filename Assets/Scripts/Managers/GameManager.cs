using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject fireworks;
    private List<ParticleSystem> fireworkPSs = new List<ParticleSystem>();

    public List<Mob> PlayerMobs { get; set; } = new List<Mob>();
    public List<Mob> EnemyMobs { get; set; } = new List<Mob>();

    public bool ControlLock { get; set; }
    public Mob PickingMob { get; set; }
    public Mob ActivatedMob { get; set; }

    private QueueManager queueManager;
    private MusicManager musicManager;
    private UIManager uiManager;

    public bool IsWin, IsLose;

    private IEnumerator calmMusicCoroutine;

    private void Awake()
    {
        queueManager = GetComponent<QueueManager>();
        musicManager = GetComponent<MusicManager>();
        uiManager = GetComponent<UIManager>();
    }

    private void Start()
    {
        fireworkPSs.AddRange(fireworks.GetComponentsInChildren<ParticleSystem>());
        uiManager.ShowAnouncerPanel(true, "Assign actions!");
        calmMusicCoroutine = musicManager.FadeTrackToZero(musicManager.Calm);
        musicManager.StartCalmMusic();
    }

    public void AddMob(Mob mob)
    {
        if (mob.IsHostile) EnemyMobs.Add(mob);
        else PlayerMobs.Add(mob);
    }

    public void MobDied(Mob mob)
    {
        if (mob.IsActivated) uiManager.UpdateCurrentMobPanel(null);
        if (mob.IsHostile) EnemyMobs.RemoveAll(x => x == mob);
        else PlayerMobs.RemoveAll(x => x == mob);
    }

    public void ReadyToFight()
    {
        foreach (Mob mob in PlayerMobs)
        {
            if (!mob.IsHaveAction) return;
        }

        StartCoroutine(calmMusicCoroutine);
        uiManager.ShowStartBattleButton();
    }

    public void StartFight()
    {
        uiManager.HideStartBattleButton();
        queueManager.CreateQueue();
        queueManager.RunQueue();
        musicManager.StartBattleMusic();
    }

    public void EndFight()
    {
        if (musicManager != null) StartCoroutine(musicManager.FadeTrackToZero(musicManager.Battle));

        foreach (Mob mob in PlayerMobs)
        { 
            mob.IsUnderDefense = false; 
            mob.GoToOriginPosition(false);
            mob.UI.HideShield();
        }

        foreach (Mob mob in EnemyMobs) 
        { 
            mob.IsUnderDefense = false;
            mob.UI.HideShield();
        }

        CheckWinCondition();
    }

    public void ResetMobs()
    {
        foreach (Mob mob in PlayerMobs)
        {
            mob.IsHaveAction = false;
        }

        foreach (Mob mob in EnemyMobs)
        {
            mob.IsHaveAction = false;
        }
    }

    private void CheckWinCondition()
    {
        if (EnemyMobs.Count <= 0)
        {
            musicManager.StartWinMusic();
            IsWin = true;
            uiManager.GameEndScreen();
            uiManager.ShowGameEndPanel("VICTORY!");
            if (fireworkPSs.Count != 0) StartCoroutine(LaunchFireworks());
            return;
        }

        if (PlayerMobs.Count <= 0)
        {
            musicManager.StartLoseMusic();
            IsLose = true;
            uiManager.GameEndScreen();
            uiManager.ShowGameEndPanel("DEFEAT!");
            return;
        }

        ResetMobs();

        uiManager.ShowAnouncerPanel(true, "Next round!");
        StopCoroutine(calmMusicCoroutine);
        musicManager.StartCalmMusic();
    }

    private IEnumerator LaunchFireworks()
    {
        for (int i = 0; i < 100;  i++)
        {
            int rand = UnityEngine.Random.Range(0, fireworkPSs.Count);
            fireworkPSs[rand].GetComponent<AudioSource>().Play();
            fireworkPSs[rand].Play();

            yield return new WaitForSeconds(UnityEngine.Random.Range(.2f, 2f));
        }
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
