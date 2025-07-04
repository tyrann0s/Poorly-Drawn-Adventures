using System.Collections;
using Cards;
using Levels;
using Managers.Base;
using Mobs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public enum SelectingState
    {
        None,
        Enemy,
        Player
    }

    public enum GamePhase
    {
        Prepare,
        AssignActions,
        Fight,
        Win,
        Lose
    }
    
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;
        public static GameManager Instance
        {
            get
            {
                if (!instance)
                {
                    instance = FindFirstObjectByType<GameManager>();
                    if (!instance)
                    {
                        GameObject go = new GameObject("GameManager");
                        instance = go.AddComponent<GameManager>();
                    }
                }
                return instance;
            }
        }
        
        public Level CurrentLevel { get; private set; }

        public bool ControlLock { get; set; }
        public SelectingState SelectingState { get; set; }
        public GamePhase CurrentPhase { get; set; }
        public Mob PickingMob { get; set; }
        public Mob ActivatedMob { get; set; }
        public int CurrentCoins { get; set; }

        private IEnumerator calmMusicCoroutine;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            CurrentLevel = ProgressManager.Instance.LevelToLoad;
        }

        private void Start()
        {
            PreparationPhase();
        }

        public void PreparationPhase()
        {
            CurrentPhase = GamePhase.Prepare;
            UIManager.Instance.ShowAssignActionsButton();
            calmMusicCoroutine = MusicManager.Instance.FadeTrackToZero(MusicManager.Instance.Calm);
            MusicManager.Instance.StartCalmMusic();

            ResetMobs();
            CardPanel.Instance.ResetRound();
            CardPanel.Instance.GenereteCards();
            UIManager.Instance.ShowChangeCardsButton();
        }

        public void AssignActionsPhase()
        {
            CurrentPhase = GamePhase.AssignActions;
            CardPanel.Instance.StopChangeMode();
            UIManager.Instance.HideChangeCardsButton();
            UIManager.Instance.HideAssignActionsButton();
        }

        public void ReadyToFight()
        {
            if (MobManager.Instance.PlayerMobs == null || MobManager.Instance.PlayerMobs.Count == 0)
            {
                Debug.LogError("No player mobs available!");
                return;
            }
            
            bool allActionsAssigned = true;
            foreach (Mob mob in MobManager.Instance.PlayerMobs)
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
            CurrentPhase = GamePhase.Fight;
            UIManager.Instance.HideStartBattleButton();
            QueueManager.Instance.CreateQueue();
            QueueManager.Instance.RunQueue();
            MusicManager.Instance.StartBattleMusic();
        }

        public void EndFight()
        {
            if (MusicManager.Instance) StartCoroutine(MusicManager.Instance.FadeTrackToZero(MusicManager.Instance.Battle));
            
            QueueManager.Instance.StopQueue();
            foreach (Mob mob in MobManager.Instance.PlayerMobs)
            { 
                if (!mob.MobMovement.IsOnOriginPosition()) mob.MobMovement.GoToOriginPosition(false);
                
                mob.State = MobState.Idle; 
                mob.MobStatusEffects.UpdateEffectsDuration();
                mob.CurrentAction.Targets.Clear();
                mob.UI.HideShield();
            }

            foreach (Mob mob in MobManager.Instance.EnemyMobs)
            {
                mob.State = MobState.Idle;
                mob.MobStatusEffects.UpdateEffectsDuration();
                mob.CurrentAction.Targets.Clear();
                mob.UI.HideShield();
            }
            
            CheckWinCondition();
            PreparationPhase();
        }

        public void ResetMobs()
        {
            foreach (Mob mob in MobManager.Instance.PlayerMobs)
            {
                mob.State = MobState.Idle;
            }

            foreach (Mob mob in MobManager.Instance.EnemyMobs)
            {
                mob.State = MobState.Idle;
            }
        }

        public void CheckWinCondition()
        {
            if (MobManager.Instance.EnemyMobs.Count <= 0)
            {
                if (MobManager.Instance.CurrentWave < MobManager.Instance.MaxWaves - 1)
                {
                    //DOTween.KillAll();
                    CurrentCoins += CurrentLevel.coinsForWave;
                    UIManager.Instance.UpdateCoins(CurrentCoins);
                    MobManager.Instance.CurrentWave++;
                    MobManager.Instance.SpawnNextWave();
                    EndFight();
                    return;
                }

                MobManager.Instance.SpawnBoss();
                return;
            }

            if (MobManager.Instance.PlayerMobs.Count <= 0)
            {
                Lose();
            }
        }

        public void Win()
        {
            MusicManager.Instance.StartWinMusic();
            CurrentPhase = GamePhase.Win;
            UIManager.Instance.GameEndScreen();
            UIManager.Instance.ShowGameEndPanel("VICTORY!");
            
            ProgressManager.Instance.Coins += CurrentCoins;
            ProgressManager.Instance.UnlockMob(CurrentLevel.rewardMob);
            ProgressManager.Instance.UnlockHero(CurrentLevel.rewardHero);
            ProgressManager.Instance.UnlockLevel(CurrentLevel.GetNextLevel());
        }

        public void Lose()
        {
            MusicManager.Instance.StartLoseMusic();
            CurrentPhase = GamePhase.Lose;
            UIManager.Instance.GameEndScreen();
            UIManager.Instance.ShowGameEndPanel("DEFEAT!");
        }

        public void BackToBase()
        {
            // TODO Удалить анлоки, они должны анлокаться только в Win
            ProgressManager.Instance.Coins += CurrentCoins;
            ProgressManager.Instance.UnlockMob(CurrentLevel.rewardMob);
            ProgressManager.Instance.UnlockHero(CurrentLevel.rewardHero);
            ProgressManager.Instance.UnlockLevel(CurrentLevel.GetNextLevel());
            SaveSystem.Instance.SaveGame();
            FindAnyObjectByType<AdvancedSceneLoader>(FindObjectsInactive.Include).LoadBase();
        }
    }
}