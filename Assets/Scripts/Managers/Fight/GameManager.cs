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
    
    public class GameManager : MonoBehaviour, IManager
    {
        public Level CurrentLevel { get; set; }

        public bool ControlLock { get; set; }
        public SelectingState SelectingState { get; set; }
        public GamePhase CurrentPhase { get; set; }
        public Mob PickingMob { get; set; }
        public Mob ActivatedMob { get; set; }
        public int CurrentCoins { get; set; }

        private IEnumerator calmMusicCoroutine;
        
        public void Initialize()
        {
            CurrentLevel = ProgressManager.Instance.LevelToLoad;
            PreparationPhase();
        }

        public void PreparationPhase()
        {
            CurrentPhase = GamePhase.Prepare;
            ServiceLocator.Get<UIManager>().ShowAssignActionsButton();
            calmMusicCoroutine = ServiceLocator.Get<MusicManager>().FadeTrackToZero(ServiceLocator.Get<MusicManager>().Calm);
            ServiceLocator.Get<MusicManager>().StartCalmMusic();

            ResetMobs();
            ServiceLocator.Get<CardPanel>().ResetRound();
            ServiceLocator.Get<CardPanel>().GenereteCards(false);
            ServiceLocator.Get<UIManager>().ShowChangeCardsButton();
        }

        public void AssignActionsPhase()
        {
            CurrentPhase = GamePhase.AssignActions;
            ServiceLocator.Get<CardPanel>().StopChangeMode();
            ServiceLocator.Get<UIManager>().HideChangeCardsButton();
            ServiceLocator.Get<UIManager>().HideAssignActionsButton();
        }

        public void ReadyToFight()
        {
            if (ServiceLocator.Get<MobManager>().PlayerMobs == null || ServiceLocator.Get<MobManager>().PlayerMobs.Count == 0)
            {
                Debug.LogError("No player mobs available!");
                return;
            }
            
            bool allActionsAssigned = true;
            foreach (Mob mob in ServiceLocator.Get<MobManager>().PlayerMobs)
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

            ServiceLocator.Get<UIManager>().ShowStartBattleButton();
        }

        public void StartFight()
        {
            CurrentPhase = GamePhase.Fight;
            ServiceLocator.Get<UIManager>().HideStartBattleButton();
            ServiceLocator.Get<QueueManager>().CreateQueue();
            ServiceLocator.Get<QueueManager>().RunQueue();
            ServiceLocator.Get<MusicManager>().StartBattleMusic();
        }

        public void EndFight()
        {
            if (ServiceLocator.Get<MusicManager>()) StartCoroutine(ServiceLocator.Get<MusicManager>().FadeTrackToZero(ServiceLocator.Get<MusicManager>().Battle));
            
            ServiceLocator.Get<QueueManager>().StopQueue();
            foreach (Mob mob in ServiceLocator.Get<MobManager>().PlayerMobs)
            { 
                if (!mob.MobMovement.IsOnOriginPosition()) mob.MobMovement.GoToOriginPosition(false);
                
                mob.State = MobState.Idle; 
                mob.MobStatusEffects.UpdateEffectsDuration();
                mob.CurrentAction.Targets.Clear();
                mob.UI.HideShield();
            }

            foreach (Mob mob in ServiceLocator.Get<MobManager>().EnemyMobs)
            {
                mob.State = MobState.Idle;
                mob.MobStatusEffects.UpdateEffectsDuration();
                mob.CurrentAction.Targets.Clear();
                mob.UI.HideShield();
            }
            
            CheckWinCondition();
        }

        public void ResetMobs()
        {
            foreach (Mob mob in ServiceLocator.Get<MobManager>().PlayerMobs)
            {
                mob.State = MobState.Idle;
            }

            foreach (Mob mob in ServiceLocator.Get<MobManager>().EnemyMobs)
            {
                mob.State = MobState.Idle;
            }
        }

        public void CheckWinCondition()
        {
            if (ServiceLocator.Get<MobManager>().EnemyMobs.Count <= 0)
            {
                if (ServiceLocator.Get<MobManager>().CurrentWave < ServiceLocator.Get<MobManager>().MaxWaves - 1)
                {
                    AddCoins();
                    ServiceLocator.Get<MobManager>().CurrentWave++;
                    ServiceLocator.Get<MobManager>().SpawnNextWave();
                    PreparationPhase();
                    return;
                }

                AddCoins();
                ServiceLocator.Get<MobManager>().SpawnBoss();
                PreparationPhase();
                return;
            }

            if (ServiceLocator.Get<MobManager>().PlayerMobs.Count <= 0)
            {
                Lose();
                return;
            }
            
            PreparationPhase();
        }

        public void Win()
        {
            ServiceLocator.Get<MusicManager>().StartWinMusic();
            CurrentPhase = GamePhase.Win;
            ServiceLocator.Get<UIManager>().GameEndScreen();
            ServiceLocator.Get<UIManager>().ShowGameEndPanel("VICTORY!");
            
            ProgressManager.Instance.Coins += CurrentCoins;
            ProgressManager.Instance.UnlockAlly(CurrentLevel.rewardMob);
            ProgressManager.Instance.UnlockHero(CurrentLevel.rewardHero);
            ProgressManager.Instance.UnlockLevel(CurrentLevel.GetNextLevel());
        }

        public void Lose()
        {
            ServiceLocator.Get<MusicManager>().StartLoseMusic();
            CurrentPhase = GamePhase.Lose;
            ServiceLocator.Get<UIManager>().GameEndScreen();
            ServiceLocator.Get<UIManager>().ShowGameEndPanel("DEFEAT!");
        }

        public void BackToBase()
        {
            SaveSystem.Instance.SaveGame();
            FindAnyObjectByType<LoadingScreen>(FindObjectsInactive.Include).LoadBase();
        }

        private void AddCoins()
        {
            CurrentCoins += CurrentLevel.coinsForWave;
            ServiceLocator.Get<UIManager>().UpdateCoins(CurrentCoins);
        }
    }
}