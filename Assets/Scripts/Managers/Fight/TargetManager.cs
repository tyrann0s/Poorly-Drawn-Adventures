using System;
using System.Collections.Generic;
using Items;
using Mobs;
using UnityEngine;

namespace Managers
{
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

        private void ClearContext()
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
        public Mob ParentMob {get; private set;}
        public ScrollItem ParentItem {get; private set;}
        public Func<Mob, bool> TargetFilter {get; private set;}
        public int MaxTargets { get; private set; } = 1;
        
        public TargetSelectionContext(
            SelectingState selectingState,
            Mob parentMob,
            ScrollItem parentItem,
            Func<Mob, bool> targetFilter,
            int maxTargets = 1)
        {
            CurrentSelectingState = selectingState;
            ParentMob = parentMob;
            ParentItem = parentItem;
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

