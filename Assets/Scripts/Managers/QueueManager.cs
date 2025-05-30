using System;
using System.Collections.Generic;
using Mobs;
using UnityEngine;

namespace Managers
{
    public class QueueManager : MonoBehaviour
    {
        private static QueueManager instance;
        public static QueueManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindFirstObjectByType<QueueManager>();
                    if (instance == null)
                    {
                        GameObject go = new GameObject("Queue Manager");
                        instance = go.AddComponent<QueueManager>();
                    }
                }

                return instance;
            }
        }
        
        private List<Action> actionList = new List<Action>();

        private int currentActionIndex;

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

        public void CreateQueue()
        {
            actionList.Clear();

            GenerateEnemyActions();

            actionList.AddRange(GetActions(GameManager.Instance.PlayerMobs, ActionType.Defense));
            actionList.AddRange(GetActions(GameManager.Instance.EnemyMobs, ActionType.Defense));

            actionList.AddRange(GetActions(GameManager.Instance.PlayerMobs, ActionType.Attack1));
            actionList.AddRange(GetActions(GameManager.Instance.PlayerMobs, ActionType.Attack2));
            actionList.AddRange(GetActions(GameManager.Instance.EnemyMobs, ActionType.Attack1));
            actionList.AddRange(GetActions(GameManager.Instance.EnemyMobs, ActionType.Attack2));

            actionList.AddRange(GetActions(GameManager.Instance.PlayerMobs, ActionType.SkipTurn));
            actionList.AddRange(GetActions(GameManager.Instance.EnemyMobs, ActionType.SkipTurn));
        }

        public void CreateQueueForActionType(ActionType actionType)
        {
            actionList.AddRange(GetActions(GameManager.Instance.PlayerMobs, actionType));
            actionList.AddRange(GetActions(GameManager.Instance.EnemyMobs, actionType));
        }

        private void GenerateEnemyActions()
        {
            if (GameManager.Instance == null)
            {
                Debug.LogError("Game Manager is null!");
                return;
            }

            if (GameManager.Instance.EnemyMobs == null)
            {
                Debug.LogError("EnemyMobs list is null!");
                return;
            }

            if (GameManager.Instance.PlayerMobs == null)
            {
                Debug.LogError("PlayerMobs list is null!");
                return;
            }

            foreach (Mob mob in GameManager.Instance.EnemyMobs)
            {
                if (mob == null || mob.State == MobState.Dead || mob.State == MobState.Stun)
                    continue;
            
                if (mob.StunTime > 0)
                {
                    mob.MobActions.PerformStun();
                    continue;
                }

                mob.CurrentAction.MobInstance = mob;
            
                // Получаем все возможные типы действий
                var actionTypes = Enum.GetValues(typeof(ActionType));
                int randActionType = UnityEngine.Random.Range(1, actionTypes.Length);
                mob.CurrentAction.MobAction = (ActionType)randActionType;

                if (mob.MobActions.CheckStamina())
                {
                    if (mob.CurrentAction.MobAction == ActionType.Attack1 || mob.CurrentAction.MobAction == ActionType.Attack2)
                    {
                        if (GameManager.Instance.PlayerMobs.Count > 0)
                        {
                            int randMob = UnityEngine.Random.Range(0, GameManager.Instance.PlayerMobs.Count);
                            mob.CurrentAction.TargetInstance = GameManager.Instance.PlayerMobs[randMob];
                        }
                    }
                }
                else
                {
                    mob.CurrentAction.MobAction = ActionType.SkipTurn;
                }
            }
        }

        private List<Action> GetActions(List<Mob> list, ActionType actionType)
        {
            if (list == null)
            {
                Debug.LogError("Mob list is null!");
                return new List<Action>();
            }

            List<Action> resultList = new List<Action>();
            foreach (Mob mob in list)
            {
                if (mob == null || mob.CurrentAction == null)
                    continue;

                if (mob.CurrentAction.MobAction == actionType)
                {
                    resultList.Add(mob.CurrentAction);
                }
            }

            return resultList;
        }

        public void RunQueue()
        {
            if (actionList == null || actionList.Count == 0)
            {
                Debug.LogError("Action list is empty!");
                return;
            }

            currentActionIndex = 0;
            PerformAction(actionList[currentActionIndex]);
        }

        public void NextAction()
        {
            currentActionIndex++;

            if (currentActionIndex >= actionList.Count)
            {
                GameManager.Instance.EndFight();
            }
            else PerformAction(actionList[currentActionIndex]);
        }

        private void PerformAction(Action action)
        {
            if (action == null || !action.MobInstance)
            {
                Debug.LogError("Invalid action or mob instance!");
                NextAction();
                return;
            }

            if (action.MobInstance.State == MobState.Dead || action.MobInstance.State == MobState.Stun)
            {
                Debug.Log($"Skipping dead mob {action.MobInstance.name}");
                NextAction();
                return;
            }
        
            if (action.MobInstance.StunTime > 0)
            {
                NextAction();
                return;
            }

            switch (action.MobAction)
            {
                case ActionType.SkipTurn:
                    action.MobInstance.MobActions.PerformSkipTurn();
                    break;

                case ActionType.Defense:
                    action.MobInstance.MobActions.PerformDefense();
                    break;

                case ActionType.Attack1:
                case ActionType.Attack2:
                    if (action.TargetInstance == null || action.TargetInstance.State == MobState.Dead || action.TargetInstance.State == MobState.Stun)
                    {
                        Debug.Log($"Attack target is dead or null for {action.MobInstance.name}");
                        NextAction();
                        return;
                    }
                    if (action.MobInstance.StunTime > 0) {}
                    else action.MobInstance.MobActions.PerformAttack();
                    break;

                default:
                    Debug.LogWarning($"Unknown action type: {action.MobAction}");
                    NextAction();
                    break;
            }
        }
    }
}
