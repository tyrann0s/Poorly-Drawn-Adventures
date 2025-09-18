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
            
            // Добавляем дополнительную проверку на начало создания очереди
            CleanupNullReferences();
            
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

        // Добавляем метод для очистки null ссылок в начале создания очереди
        private void CleanupNullReferences()
        {
            var mobManager = ServiceLocator.Get<MobManager>();
            if (mobManager != null)
            {
                mobManager.CleanupAllDestroyedMobs();
            }
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
                // Добавляем дополнительную проверку на null и уничтоженный объект
                if (!mob || mob.gameObject == null || mob.State == MobState.Dead) 
                {
                    Debug.LogWarning($"Skipping null or destroyed mob in GenerateEnemyActions");
                    continue;
                }
                
                mob.CurrentAction.MobInstance = mob;

                // Если моб хилит ищем кого похилить
                if (mob.MobData.ActiveSkill is HealSkill)
                {
                    mob.CurrentAction.MobActionType = ActionType.ActiveSkill;

                    if (mob.MobActions.CheckStamina())
                    {
                        if (GetEnemiesWounded().Count > 0)
                        {
                            int maxTargets = GetEnemiesWounded().Count > mob.MobData.MaxTargets ? mob.MobData.MaxTargets : GetEnemiesWounded().Count;
                            for (int i = 0; i < maxTargets; i++)
                            {
                               if (mob.State != MobState.Dead) mob.CurrentAction.Targets.Add(GetEnemiesWounded()[i]);
                            }
                        }
                    }
                    else
                    {
                        mob.CurrentAction.MobActionType = ActionType.Attack;
                        if (!mob.MobActions.CheckStamina()) mob.CurrentAction.MobActionType = ActionType.SkipTurn;
                    }
                    
                    continue;
                }

                // Если моб ресает ищем кого реснуть
                if (mob.MobData.ActiveSkill is ResurrectSkill)
                {
                    mob.CurrentAction.MobActionType = ActionType.ActiveSkill;

                    if (mob.MobActions.CheckStamina())
                    {
                        if (GetEnemiesDead().Count > 0)
                        {
                            if (GetEnemiesDead().Count > 0)
                            {
                                int maxTargets = GetEnemiesDead().Count > mob.MobData.MaxTargets ? mob.MobData.MaxTargets : GetEnemiesDead().Count;
                                for (int i = 0; i < maxTargets; i++)
                                {
                                    if (mob.State != MobState.Dead) mob.CurrentAction.Targets.Add(GetEnemiesDead()[i]);
                                }
                            }
                        }
                    }
                    else
                    {
                        mob.CurrentAction.MobActionType = ActionType.Attack;
                        if (!mob.MobActions.CheckStamina()) mob.CurrentAction.MobActionType = ActionType.SkipTurn;
                    }
                    
                    continue;
                }
                
                var actionTypes = Enum.GetValues(typeof(ActionType));
                int randActionType = UnityEngine.Random.Range(1, actionTypes.Length);
                mob.CurrentAction.MobActionType = (ActionType)randActionType;

                if (mob.MobActions.CheckStamina())
                {
                    if (mob.CurrentAction.MobActionType is ActionType.ActiveSkill)
                    {
                        if (ServiceLocator.Get<MobManager>().PlayerMobs.Count > 0)
                        {
                            List<Mob> targets = new List<Mob>();
                            foreach (var playerMob in ServiceLocator.Get<MobManager>().PlayerMobs)
                            {
                                if (playerMob.State != MobState.Dead) targets.Add(playerMob);
                            }

                            if (targets.Count > 0)
                            {
                                for (int i = 0; i < mob.MobData.MaxTargets; i++)
                                {
                                    int randIndex = UnityEngine.Random.Range(0, targets.Count);
                                    mob.CurrentAction.Targets.Add(targets[randIndex]);
                                }
                            }
                        }
                    }
                    
                    if (mob.CurrentAction.MobActionType is ActionType.Attack)
                    {
                        int randIndex = UnityEngine.Random.Range(0, ServiceLocator.Get<MobManager>().PlayerMobs.Count);
                        mob.CurrentAction.Targets.Add(ServiceLocator.Get<MobManager>().PlayerMobs[randIndex]); 
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
                StartCoroutine(ServiceLocator.Get<GameManager>().EndFight());
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
                Debug.Log($"Mob {action.MobInstance.name} is stunned, skipping turn");
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
                    if (action.TargetInstance)
                    {
                        if (action.MobInstance.MobData.ActiveSkill is ResurrectSkill && action.TargetInstance.State == MobState.Dead)
                        {
                            action.MobInstance.CurrentAction.TargetInstance = action.TargetInstance;
                            action.MobInstance.MobActions.PrepareAction(action.MobActionType);
                            break;
                        } 
                        
                        if (action.TargetInstance.State == MobState.Dead)
                        {
                            Debug.Log($"Attack target is dead for {action.MobInstance.name}");
                            NextAction();
                            break;
                        }
                        
                        action.MobInstance.CurrentAction.TargetInstance = action.TargetInstance;
                        action.MobInstance.MobActions.PrepareAction(action.MobActionType);
                    }
                    else
                    {
                        Debug.LogWarning($"No target for action {action.MobActionType} by {action.MobInstance.name}");
                        NextAction(); // ✅ Исправление
                    }
                    break;
                
                case ActionType.PassiveSkill:
                    if (action.MobInstance.MobStatusEffects.StatusEffects.Any(x=>x.EffectType == StatusEffectType.Stun)) 
                    {
                        Debug.Log($"Mob {action.MobInstance.name} is stunned during passive skill");
                        NextAction(); // ✅ Исправление
                    }
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

        private List<Mob> GetEnemiesWounded()
        {
            var woundedMobs = new List<Mob>();
            foreach (var enemyMob in ServiceLocator.Get<MobManager>().EnemyMobs)
            {
                if (enemyMob.MobHP < enemyMob.MobData.MaxHP && enemyMob.State != MobState.Dead)
                {
                    woundedMobs.Add(enemyMob);
                }
            }

            return woundedMobs;
        }

        private List<Mob> GetEnemiesDead()
        {
            var deadMobs = new List<Mob>();
            foreach (var enemyMob in ServiceLocator.Get<MobManager>().EnemyMobs)
            {
                if (enemyMob.State == MobState.Dead)
                {
                    deadMobs.Add(enemyMob);
                }
            }

            return deadMobs;
        }
    }
}