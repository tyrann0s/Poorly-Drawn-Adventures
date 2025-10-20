using System;
using System.Collections.Generic;
using Items;
using Mobs;
using UnityEngine;

namespace Managers
{
    public enum SourceType
    {
        MobControl,
        MobTarget,
        ItemTarget
    }
    
    public class TargetManager : MonoBehaviour, IManager
    {
        public TargetSelectionContext CurrentContext { get; private set; }
        public List<Mob> Targets { get; private set; } = new();
        
        public void Initialize()
        {

        }

        public void SetContext(TargetSelectionContext context)
        {
            ClearContext();
            CurrentContext = context;
        }

        public void ClearContext()
        {
            CurrentContext = null;
            Targets.Clear();
        }

        public void AddTarget(Mob target)
        {
            Targets.Add(target);
        }
    }

    public class TargetSelectionContext
    {
        public SelectingState CurrentSelectingState { get; private set; }
        public SourceType CurrentSourceType { get; private set; }
        public Func<Mob, bool> TargetFilter {get; private set;}
        public int MaxTargets { get; private set; } = 1;
        
        public TargetSelectionContext(
            SourceType sourceType,
            SelectingState selectingState,
            Func<Mob, bool> targetFilter,
            int maxTargets = 1)
        {
            CurrentSourceType = sourceType;
            CurrentSelectingState = selectingState;
            TargetFilter = targetFilter;
            MaxTargets = maxTargets;
        }

        public bool CanSelectTarget(Mob mob)
        {
            // дополнительные условия для фильтрации
            return TargetFilter(mob);
        }
    }
}

