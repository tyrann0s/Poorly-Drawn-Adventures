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
        private List<MobAction> actionList = new List<MobAction>();

        private int currentActionIndex;

        public void CreateQueue()
        {
            actionList.Clear();
            GenerateEnemyActions();

            // Фаза защиты
            var defensePlayer = GetDefenseActions(ServiceLocator.Get<MobManager>().PlayerMobs);
            var defenseEnemy = GetDefenseActions(ServiceLocator.Get<MobManager>().EnemyMobs);
            actionList.AddRange(defensePlayer);
            actionList.AddRange(defenseEnemy);

            // Фаза скиллов поддержки
            var supportPlayer = GetSupportActions(ServiceLocator.Get<MobManager>().PlayerMobs);
            var supportEnemy = GetSupportActions(ServiceLocator.Get<MobManager>().EnemyMobs);
            actionList.AddRange(supportPlayer);
            actionList.AddRange(supportEnemy);
            
            // Фаза атаки
            var attackPlayer = GetAttackActions(ServiceLocator.Get<MobManager>().PlayerMobs);
            var attackEnemy = GetAttackActions(ServiceLocator.Get<MobManager>().EnemyMobs);
            actionList.AddRange(attackPlayer);
            actionList.AddRange(attackEnemy);

            // Фаза пропуска хода
            var skipPlayer = GetSkipTurnActions(ServiceLocator.Get<MobManager>().PlayerMobs);
            var skipEnemy = GetSkipTurnActions(ServiceLocator.Get<MobManager>().EnemyMobs);
            actionList.AddRange(skipPlayer);
            actionList.AddRange(skipEnemy);
        }

        private void GenerateEnemyActions()
        {
            if (!ServiceLocator.Get<GameManager>())
            {
                Debug.LogError("Game Manager is null!");
                return;
            }

            if (ServiceLocator.Get<MobManager>().EnemyMobs == null)
            {
                Debug.LogError("EnemyMobs list is null!");
                return;
            }

            if (ServiceLocator.Get<MobManager>().PlayerMobs == null)
            {
                Debug.LogError("PlayerMobs list is null!");
                return;
            }

            foreach (Mob mob in ServiceLocator.Get<MobManager>().EnemyMobs)
            {
                if (!mob || mob.State == MobState.Dead) continue;

                mob.CurrentAction.MobInstance = mob;
            
                // Получаем все возможные типы действий
                var actionTypes = Enum.GetValues(typeof(ActionType));
                int randActionType = UnityEngine.Random.Range(1, actionTypes.Length);
                mob.CurrentAction.MobActionType = (ActionType)randActionType;

                if (mob.MobActions.CheckStamina())
                {
                    if (mob.CurrentAction.MobActionType is ActionType.Attack or ActionType.ActiveSkill)
                    {
                        if (ServiceLocator.Get<MobManager>().PlayerMobs.Count > 0)
                        {
                            int randMob = UnityEngine.Random.Range(0, ServiceLocator.Get<MobManager>().PlayerMobs.Count);
                            mob.CurrentAction.Targets.Add(ServiceLocator.Get<MobManager>().PlayerMobs[randMob]); 
                        }
                    }
                }
                else
                {
                    mob.CurrentAction.MobActionType = ActionType.SkipTurn;
                }
            }
        }

        private List<MobAction> GetDefenseActions(List<Mob> list)
        {
            List<MobAction> resultList = new List<MobAction>();
            foreach (Mob mob in list)
            {
                if (!mob || mob.CurrentAction == null)
                    continue;

                if (mob.CurrentAction.MobActionType == ActionType.Defense)
                {
                    resultList.Add(mob.CurrentAction);
                }
            }

            return resultList;
        }

        private List<MobAction> GetSupportActions(List<Mob> list)
        {
            List<MobAction> resultList = new List<MobAction>();
            foreach (Mob mob in list)
            {
                if (!mob || mob.CurrentAction == null)
                    continue;
                
                if (mob.CurrentAction.MobActionType == ActionType.ActiveSkill)
                {
                    if (!mob.MobData.ActiveSkill.IsAttack)
                    {
                        foreach (Mob actionTarget in mob.CurrentAction.Targets)
                        {
                            resultList.Add(new MobAction(mob.CurrentAction.MobActionType, mob, actionTarget));;
                        }
                    }
                }
            }

            return resultList;
        }

        private List<MobAction> GetAttackActions(List<Mob> list)
        {
            List<MobAction> resultList = new List<MobAction>();
            foreach (Mob mob in list)
            {
                if (!mob || mob.CurrentAction == null)
                    continue;

                if (mob.CurrentAction.MobActionType == ActionType.Attack)
                {
                    foreach (Mob actionTarget in mob.CurrentAction.Targets)
                    {
                        resultList.Add(new MobAction(mob.CurrentAction.MobActionType, mob, actionTarget));;
                    }
                    continue;
                }
                
                if (mob.CurrentAction.MobActionType == ActionType.ActiveSkill)
                {
                    if (mob.MobData.ActiveSkill.IsAttack)
                    {
                        foreach (Mob actionTarget in mob.CurrentAction.Targets)
                        {
                            resultList.Add(new MobAction(mob.CurrentAction.MobActionType, mob, actionTarget));;
                        }
                    }
                }
            }

            return resultList;
        }

        private List<MobAction> GetSkipTurnActions(List<Mob> list)
        {
            List<MobAction> resultList = new List<MobAction>();
            foreach (Mob mob in list)
            {
                if (!mob || mob.CurrentAction == null)
                    continue;
                
                if (mob.CurrentAction.MobActionType == ActionType.SkipTurn) resultList.Add(mob.CurrentAction);
            }

            return resultList;
        }

        public void InjectAction(MobAction action)
        {
            actionList.Insert(currentActionIndex + 1, action);
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

        public void StopQueue()
        {
            currentActionIndex = 0;
        }

        public void NextAction()
        {
            currentActionIndex++;

            if (currentActionIndex >= actionList.Count)
            {
                ServiceLocator.Get<GameManager>().EndFight();
            }
            else PerformAction(actionList[currentActionIndex]);
        }

        private void PerformAction(MobAction action)
        {
            if (ServiceLocator.Get<GameManager>().CurrentPhase != GamePhase.Fight) return;
            
            if (action == null || !action.MobInstance)
            {
                Debug.LogError("Invalid action or mob instance!");
                NextAction();
                return;
            }

            if (action.MobInstance.State == MobState.Dead)
            {
                Debug.Log($"Skipping dead mob {action.MobInstance.name}");
                NextAction();
                return;
            }
        
            if (action.MobInstance.MobStatusEffects.CheckStun())
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
                case ActionType.ActiveSkill:
                    if (!action.TargetInstance || action.TargetInstance.MobStatusEffects.CheckStun())
                    {
                        if (action.MobInstance.MobData.ActiveSkill is ResurrectSkill && action.TargetInstance.State == MobState.Dead)
                        {
                            action.MobInstance.CurrentAction.TargetInstance = action.TargetInstance;
                            action.MobInstance.MobActions.PrepareAction(action.MobActionType);
                        } else if (action.MobInstance.CurrentAction.TargetInstance.State == MobState.Dead)
                        {
                            Debug.Log($"Attack target is dead or null for {action.MobInstance.name}");
                            NextAction();
                        }
                        return;
                    }
                    if (action.MobInstance.MobStatusEffects.StatusEffects.Any(x=>x.EffectType == StatusEffectType.Stun)) {}
                    else
                    {
                        action.MobInstance.CurrentAction.TargetInstance = action.TargetInstance;
                        action.MobInstance.MobActions.PrepareAction(action.MobActionType);
                    }
                    break;
                case ActionType.PassiveSkill:
                    if (action.MobInstance.MobStatusEffects.StatusEffects.Any(x=>x.EffectType == StatusEffectType.Stun)) {}
                    else
                    {
                        action.MobInstance.PassiveAction.TargetInstance = action.TargetInstance;
                        action.MobInstance.MobActions.PrepareAction(action.MobActionType);
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