using System.Collections;
using System.Collections.Generic;
using Cards;
using Mobs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;
        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindFirstObjectByType<GameManager>();
                    if (instance == null)
                    {
                        GameObject go = new GameObject("GameManager");
                        instance = go.AddComponent<GameManager>();
                    }
                }
                return instance;
            }
        }

        [SerializeField]
        private GameObject fireworks;
        private List<ParticleSystem> fireworkPSs = new List<ParticleSystem>();

        public List<Mob> PlayerMobs { get; set; } = new List<Mob>();
        public List<Mob> EnemyMobs { get; set; } = new List<Mob>();

        public bool ControlLock { get; set; }
        public bool ChangeCardMode { get; set; }

        public Mob PickingMob { get; set; }
        public Mob ActivatedMob { get; set; }

        public bool IsWin { get; private set; }
        public bool IsLose { get; private set; }

        private IEnumerator calmMusicCoroutine;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            fireworkPSs.AddRange(fireworks.GetComponentsInChildren<ParticleSystem>());
            UIManager.Instance.ShowAnouncerPanel(true, "Assign actions!");
            calmMusicCoroutine = MusicManager.Instance.FadeTrackToZero(MusicManager.Instance.Calm);
            MusicManager.Instance.StartCalmMusic();

            CardPanel.Instance.GenereteCards();
            SetCardPanel(false);
            UIManager.Instance.ShowChangeCardsButton();
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

            if (mob.State == MobState.Activated)
            {
                mob.State = MobState.Idle;
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
                if (mob.State == MobState.Idle)
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

            UIManager.Instance.ShowStartBattleButton();
        }

        public void StartFight()
        {
            UIManager.Instance.HideStartBattleButton();
            QueueManager.Instance.CreateQueue();
            QueueManager.Instance.RunQueue();
            MusicManager.Instance.StartBattleMusic();
        }

        public void EndFight()
        {
            if (MusicManager.Instance != null) StartCoroutine(MusicManager.Instance.FadeTrackToZero(MusicManager.Instance.Battle));

            foreach (Mob mob in PlayerMobs)
            { 
                mob.State = MobState.Idle; 
                mob.MobMovement.GoToOriginPosition(false);
                mob.UI.HideShield();
            }

            foreach (Mob mob in EnemyMobs)
            {
                mob.State = MobState.Idle;
                mob.UI.HideShield();
            }

            // Показываем сообщение о конце раунда и сбрасываем действия мобов
            UIManager.Instance.ShowAnouncerPanel(true, "Next Round!");
            ResetMobs();
        
            // Сбрасываем состояние карт для следующего раунда
            CardPanel.Instance.ResetRound();
        
            CheckWinCondition();
        }

        public void ResetMobs()
        {
            foreach (Mob mob in PlayerMobs)
            {
                mob.State = MobState.Idle;
            }

            foreach (Mob mob in EnemyMobs)
            {
                mob.State = MobState.Idle;
            }
        }

        private void CheckWinCondition()
        {
            if (EnemyMobs.Count <= 0)
            {
                MusicManager.Instance.StartWinMusic();
                IsWin = true;
                UIManager.Instance.GameEndScreen();
                UIManager.Instance.ShowGameEndPanel("VICTORY!");
                if (fireworkPSs.Count != 0) StartCoroutine(LaunchFireworks());
                return;
            }

            if (PlayerMobs.Count <= 0)
            {
                MusicManager.Instance.StartLoseMusic();
                IsLose = true;
                UIManager.Instance.GameEndScreen();
                UIManager.Instance.ShowGameEndPanel("DEFEAT!");
                return;
            }
        
            StopCoroutine(calmMusicCoroutine);
            MusicManager.Instance.StartCalmMusic();
            CardPanel.Instance.GenereteCards();
            SetCardPanel(false);
            if (CardPanel.Instance.CanChangeCards())
            {
                UIManager.Instance.ShowChangeCardsButton();
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
                int rand = Random.Range(0, fireworkPSs.Count);
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

                yield return new WaitForSeconds(Random.Range(.2f, 2f));
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
                CardPanel.Instance.EnableInteraction();
            } else CardPanel.Instance.DisableInteraction();
        }

        public void StartChangeCardMode()
        {
            if (ChangeCardMode)
            {
                StopChangeCardMode();
                return;
            }

            CardPanel.Instance.StartChangeMode();
            ChangeCardMode = true;

            UIManager.Instance.WaitChangeCardsButton();
        }

        public void StopChangeCardMode()
        {
            CardPanel.Instance.StopChangeMode();
            UIManager.Instance.ShowChangeCardsButton();
            ChangeCardMode = false;
        }

        public void ExitChangeCardMode()
        {
            CardPanel.Instance.StopChangeMode();
            UIManager.Instance.HideChangeCardsButton();
            ChangeCardMode = false;
        }

        public ElementCombo GetCombo()
        {
            return CardPanel.Instance.GetCombo();
        }
    }
}