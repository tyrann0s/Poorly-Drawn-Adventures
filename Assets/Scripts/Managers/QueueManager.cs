using System;
using System.Collections.Generic;
using System.Linq;
using Mobs;
using Mobs.Status_Effects;
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
        
        private List<MobAction> actionList = new List<MobAction>();

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

            // Фаза защиты
            actionList.AddRange(GetActions(GameManager.Instance.PlayerMobs, ActionType.Defense));
            actionList.AddRange(GetActions(GameManager.Instance.EnemyMobs, ActionType.Defense));

            // Фаза атаки и скиллов
            actionList.AddRange(GetActions(GameManager.Instance.PlayerMobs, ActionType.Attack));
            actionList.AddRange(GetActions(GameManager.Instance.PlayerMobs, ActionType.Skill));
            actionList.AddRange(GetActions(GameManager.Instance.EnemyMobs, ActionType.Attack));
            actionList.AddRange(GetActions(GameManager.Instance.EnemyMobs, ActionType.Skill));

            // Фаза пропуска хода
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
                if (mob == null || mob.State == MobState.Dead || mob.MobStatusEffects.CheckStun())
                    continue;
            
                if (mob.MobStatusEffects.StatusEffects.Any(x=>x.EffectType == StatusEffectType.Stun))
                {
                    mob.MobActions.PerformStun();
                    continue;
                }

                mob.CurrentAction.MobInstance = mob;
            
                // Получаем все возможные типы действий
                var actionTypes = Enum.GetValues(typeof(ActionType));
                int randActionType = UnityEngine.Random.Range(1, actionTypes.Length);
                mob.CurrentAction.MobActionType = (ActionType)randActionType;

                if (mob.MobActions.CheckStamina())
                {
                    if (mob.CurrentAction.MobActionType == ActionType.Attack || mob.CurrentAction.MobActionType == ActionType.Skill)
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
                    mob.CurrentAction.MobActionType = ActionType.SkipTurn;
                }
            }
        }

        private List<MobAction> GetActions(List<Mob> list, ActionType actionType)
        {
            if (list == null)
            {
                Debug.LogError("Mob list is null!");
                return new List<MobAction>();
            }

            List<MobAction> resultList = new List<MobAction>();
            foreach (Mob mob in list)
            {
                if (mob == null || mob.CurrentAction == null)
                    continue;

                if (mob.CurrentAction.MobActionType == actionType)
                {
                    switch (mob.CurrentAction.MobActionType)
                    {
                        case ActionType.SkipTurn:
                            resultList.Add(mob.CurrentAction);
                            break;
                        case ActionType.Defense:
                            resultList.Add(mob.CurrentAction);
                            break;
                        case ActionType.Attack:
                            foreach (Mob actionTarget in mob.CurrentAction.Targets)
                            {
                                resultList.Add(new MobAction(mob.CurrentAction.MobActionType, mob, actionTarget));;
                            }
                            break;
                        case ActionType.Skill:
                            foreach (Mob actionTarget in mob.CurrentAction.Targets)
                            {
                                resultList.Add(new MobAction(mob.CurrentAction.MobActionType, mob, actionTarget));;
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
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

        private void PerformAction(MobAction action)
        {
            if (action == null || !action.MobInstance)
            {
                Debug.LogError("Invalid action or mob instance!");
                NextAction();
                return;
            }

            if (action.MobInstance.State == MobState.Dead || action.MobInstance.MobStatusEffects.CheckStun())
            {
                Debug.Log($"Skipping dead mob {action.MobInstance.name}");
                NextAction();
                return;
            }
        
            if (action.MobInstance.MobStatusEffects.StatusEffects.Any(x=>x.EffectType == StatusEffectType.Stun))
            {
                NextAction();
                return;
            }

            switch (action.MobActionType)
            {
                case ActionType.SkipTurn:
                    action.MobInstance.MobActions.PerformSkipTurn();
                    break;

                case ActionType.Defense:
                    action.MobInstance.MobActions.PerformDefense();
                    break;

                case ActionType.Attack:
                case ActionType.Skill:
                    if (!action.TargetInstance || action.TargetInstance.State == MobState.Dead || action.TargetInstance.MobStatusEffects.CheckStun())
                    {
                        Debug.Log($"Attack target is dead or null for {action.MobInstance.name}");
                        NextAction();
                        return;
                    }
                    if (action.MobInstance.MobStatusEffects.StatusEffects.Any(x=>x.EffectType == StatusEffectType.Stun)) {}
                    else
                    {
                        action.MobInstance.CurrentAction.TargetInstance = action.TargetInstance;
                        if (action.MobActionType == ActionType.Attack)
                        {
                            action.MobInstance.MobActions.PerformAttack();
                        } else if (action.MobActionType == ActionType.Skill)
                        {
                            action.MobInstance.MobActions.PerformSkill();
                        }
                    }
                    break;

                default:
                    Debug.LogWarning($"Unknown action type: {action.MobActionType}");
                    NextAction();
                    break;
            }
        }
    }
}
