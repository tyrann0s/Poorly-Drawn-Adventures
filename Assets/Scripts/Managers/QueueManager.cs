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
    }

    public void CreateQueue()
    {
        actionList.Clear();

        GenereateEnemyActions();

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

    private void GenereateEnemyActions()
    {
        foreach (Mob mob in gameManager.EnemyMobs)
        {
            if (!mob.IsDead)
            {
                mob.CurrentAction.MobInstance = mob;
                int randActionType = UnityEngine.Random.Range(1, Enum.GetValues(typeof(ActionType)).Length);
                mob.CurrentAction.MobAction = (ActionType)randActionType;

                if (mob.CheckStamina())
                {
                    if (mob.CurrentAction.MobAction == ActionType.Attack1 || mob.CurrentAction.MobAction == ActionType.Attack2)
                    {
                        int randMob = UnityEngine.Random.Range(0, gameManager.PlayerMobs.Count);
                        mob.CurrentAction.TargetInstance = gameManager.PlayerMobs[randMob];
                    }
                }
                else mob.CurrentAction.MobAction = ActionType.SkipTurn;
            }
        }
    }

    private List<Action> GetActions(List<Mob> list, ActionType actionType)
    {
        List<Action> resultList = new List<Action>();
        foreach (Mob mob in list)
        {
            if (mob.CurrentAction.MobAction == actionType) actionList.Add(mob.CurrentAction);
        }

        return resultList;
    }

    public void RunQueue()
    {
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
        if (!action.MobInstance.IsDead)
        {
            switch (action.MobAction)
            {
                case ActionType.SkipTurn:
                    action.MobInstance.PerformSkipTurn();
                    break;
                case ActionType.Defense:
                    action.MobInstance.PerformDefense();
                    break;
                case ActionType.Attack1:
                    if (!action.TargetInstance.IsDead) action.MobInstance.PerformAttack();
                    else NextAction();
                    break;
                case ActionType.Attack2:
                    if (!action.TargetInstance.IsDead) action.MobInstance.PerformAttack();
                    else NextAction();
                    break;
            }
        } else NextAction();
    }
}
