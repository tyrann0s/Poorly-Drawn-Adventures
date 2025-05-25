using System.Collections;
using System.Collections.Generic;
using Cards;
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
    public bool ChangeCardMode { get; set; }

    public Mob PickingMob { get; set; }
    public Mob ActivatedMob { get; set; }

    private QueueManager queueManager;
    private MusicManager musicManager;
    private UIManager uiManager;

    private CardPanel cardPanel;
    public CardPanel _CardPanel => cardPanel;

    public bool IsWin, IsLose;

    private IEnumerator calmMusicCoroutine;

    private void Awake()
    {
        queueManager = GetComponent<QueueManager>();
        musicManager = GetComponent<MusicManager>();
        uiManager = GetComponent<UIManager>();
        cardPanel = FindFirstObjectByType<CardPanel>();

        if (queueManager == null) Debug.LogError("Queue Manager not found!");
        if (musicManager == null) Debug.LogError("Music Manager not found!");
        if (uiManager == null) Debug.LogError("UI Manager not found!");
        if (cardPanel == null) Debug.LogError("Card Panel not found!");
    }

    private void Start()
    {
        fireworkPSs.AddRange(fireworks.GetComponentsInChildren<ParticleSystem>());
        uiManager.ShowAnouncerPanel(true, "Assign actions!");
        calmMusicCoroutine = musicManager.FadeTrackToZero(musicManager.Calm);
        musicManager.StartCalmMusic();

        cardPanel.GenereteCards();
        SetCardPanel(false);
        uiManager.ShowChangeCardsButton();
    }

    public void AddMob(Mob mob)
    {
        if (mob == null)
        {
            Debug.LogError("Trying to add null mob!");
            return;
        }

        if (mob.IsHostile)
        {
            if (!EnemyMobs.Contains(mob))
            {
                EnemyMobs.Add(mob);
            }
            else
            {
                Debug.LogWarning($"Mob {mob.name} is already in enemy mobs list!");
            }
        }
        else
        {
            if (!PlayerMobs.Contains(mob))
            {
                PlayerMobs.Add(mob);
            }
            else
            {
                Debug.LogWarning($"Mob {mob.name} is already in player mobs list!");
            }
        }
    }

    public void MobDied(Mob mob)
    {
        if (mob == null)
        {
            Debug.LogError("Trying to remove null mob!");
            return;
        }

        if (mob.IsActivated)
        {
            uiManager.UpdateCurrentMobPanel(null);
            mob.IsActivated = false;
        }

        if (mob.IsHostile)
        {
            if (EnemyMobs.Contains(mob))
            {
                EnemyMobs.Remove(mob);
            }
            else
            {
                Debug.LogWarning($"Mob {mob.name} not found in enemy mobs list!");
            }
        }
        else
        {
            if (PlayerMobs.Contains(mob))
            {
                PlayerMobs.Remove(mob);
            }
            else
            {
                Debug.LogWarning($"Mob {mob.name} not found in player mobs list!");
            }
        }

        CheckWinCondition();
    }

    public void ReadyToFight()
    {
        if (PlayerMobs == null || PlayerMobs.Count == 0)
        {
            Debug.LogError("No player mobs available!");
            return;
        }

        bool allActionsAssigned = true;
        foreach (Mob mob in PlayerMobs)
        {
            if (!mob.IsHaveAction)
            {
                allActionsAssigned = false;
                break;
            }
        }

        if (!allActionsAssigned)
        {
            Debug.Log("Waiting for all player mobs to assign actions...");
            return;
        }

        if (calmMusicCoroutine != null)
        {
            StartCoroutine(calmMusicCoroutine);
        }

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

        // Показываем сообщение о конце раунда и сбрасываем действия мобов
        uiManager.ShowAnouncerPanel(true, "Next Round!");
        ResetMobs();
        
        // Сбрасываем состояние карт для следующего раунда
        cardPanel.ResetRound();
        
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
        
        StopCoroutine(calmMusicCoroutine);
        musicManager.StartCalmMusic();
        cardPanel.GenereteCards();
        SetCardPanel(false);
        if (cardPanel.CanChangeCards())
        {
            uiManager.ShowChangeCardsButton();
        }
    }

    private IEnumerator LaunchFireworks()
    {
        if (fireworkPSs.Count == 0)
        {
            Debug.LogError("No fireworks found!");
            yield break;
        }

        for (int i = 0; i < 100;  i++)
        {
            int rand = UnityEngine.Random.Range(0, fireworkPSs.Count);
            if (rand < fireworkPSs.Count)
            {
                var firework = fireworkPSs[rand];
                if (firework != null)
                {
                    var audioSource = firework.GetComponent<AudioSource>();
                    if (audioSource != null)
                    {
                        audioSource.Play();
                    }
                    firework.Play();
                }
            }

            yield return new WaitForSeconds(UnityEngine.Random.Range(.2f, 2f));
        }
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetCardPanel(bool state)
    {
        if (state)
        {
            cardPanel.EnableInteraction();
        } else cardPanel.DisableInteraction();
    }

    public void StartChangeCardMode()
    {
        if (ChangeCardMode)
        {
            StopChangeCardMode();
            return;
        }

        cardPanel.StartChangeMode();
        ChangeCardMode = true;

        uiManager.WaitChangeCardsButton();
    }

    public void StopChangeCardMode()
    {
        cardPanel.StopChangeMode();
        uiManager.ShowChangeCardsButton();
        ChangeCardMode = false;
    }

    public void ExitChangeCardMode()
    {
        cardPanel.StopChangeMode();
        uiManager.HideChangeCardsButton();
        ChangeCardMode = false;
    }

    public ElementCombo GetCombo()
    {
        return cardPanel.GetCombo();
    }
}
