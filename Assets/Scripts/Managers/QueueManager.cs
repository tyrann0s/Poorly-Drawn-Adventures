using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    private List<Action> actionList = new List<Action>();

    private GameManager gameManager;

    private int currentActionIndex;

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("Game Manager not found!");
        }
    }

    public void CreateQueue()
    {
        actionList.Clear();

        GenerateEnemyActions();

        actionList.AddRange(GetActions(gameManager.PlayerMobs, ActionType.Defense));
        actionList.AddRange(GetActions(gameManager.EnemyMobs, ActionType.Defense));

        actionList.AddRange(GetActions(gameManager.PlayerMobs, ActionType.Attack1));
        actionList.AddRange(GetActions(gameManager.PlayerMobs, ActionType.Attack2));
        actionList.AddRange(GetActions(gameManager.EnemyMobs, ActionType.Attack1));
        actionList.AddRange(GetActions(gameManager.EnemyMobs, ActionType.Attack2));

        actionList.AddRange(GetActions(gameManager.PlayerMobs, ActionType.SkipTurn));
        actionList.AddRange(GetActions(gameManager.EnemyMobs, ActionType.SkipTurn));
    }

    public void CreateQueueForActionType(ActionType actionType)
    {
        actionList.AddRange(GetActions(gameManager.PlayerMobs, actionType));
        actionList.AddRange(GetActions(gameManager.EnemyMobs, actionType));
    }

    private void GenerateEnemyActions()
    {
        if (gameManager == null)
        {
            Debug.LogError("Game Manager is null!");
            return;
        }

        if (gameManager.EnemyMobs == null)
        {
            Debug.LogError("EnemyMobs list is null!");
            return;
        }

        if (gameManager.PlayerMobs == null)
        {
            Debug.LogError("PlayerMobs list is null!");
            return;
        }

        foreach (Mob mob in gameManager.EnemyMobs)
        {
            if (mob == null || mob.IsDead)
                continue;

            mob.CurrentAction.MobInstance = mob;
            
            // Получаем все возможные типы действий
            var actionTypes = Enum.GetValues(typeof(ActionType));
            int randActionType = UnityEngine.Random.Range(1, actionTypes.Length);
            mob.CurrentAction.MobAction = (ActionType)randActionType;

            if (mob.CheckStamina())
            {
                if (mob.CurrentAction.MobAction == ActionType.Attack1 || mob.CurrentAction.MobAction == ActionType.Attack2)
                {
                    if (gameManager.PlayerMobs.Count > 0)
                    {
                        int randMob = UnityEngine.Random.Range(0, gameManager.PlayerMobs.Count);
                        mob.CurrentAction.TargetInstance = gameManager.PlayerMobs[randMob];
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
            gameManager.EndFight();
        }
        else PerformAction(actionList[currentActionIndex]);
    }

    private void PerformAction(Action action)
    {
        if (action == null || action.MobInstance == null)
        {
            Debug.LogError("Invalid action or mob instance!");
            NextAction();
            return;
        }

        if (action.MobInstance.IsDead)
        {
            Debug.Log($"Skipping dead mob {action.MobInstance.name}");
            NextAction();
            return;
        }

        switch (action.MobAction)
        {
            case ActionType.SkipTurn:
                action.MobInstance.PerformSkipTurn();
                break;

            case ActionType.Defense:
                action.MobInstance.PerformDefense();
                break;

            case ActionType.Attack1:
            case ActionType.Attack2:
                if (action.TargetInstance == null || action.TargetInstance.IsDead)
                {
                    Debug.Log($"Attack target is dead or null for {action.MobInstance.name}");
                    NextAction();
                    return;
                }
                action.MobInstance.PerformAttack();
                break;

            default:
                Debug.LogWarning($"Unknown action type: {action.MobAction}");
                NextAction();
                break;
        }
    }
}
