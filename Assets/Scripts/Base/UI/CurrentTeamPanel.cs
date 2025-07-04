using System;
using System.Collections.Generic;
using Managers.Base;
using Mobs;
using UnityEngine;

namespace Base.UI
{
    public class CurrentTeamPanel : MonoBehaviour, IMobPanel
    {
        public List<MobElement> MobElements { get; set; } = new();

        private void Awake()
        {
            MobElements.AddRange(gameObject.GetComponentsInChildren<MobElement>());
            foreach (var mobElement in MobElements)
            {
                mobElement.gameObject.AddComponent<DraggedTarget>();   
            }
        }

        public void UpdateTeamPanel()
        {
            for (int i = 0; i < MobElements.Count; i++)
            {
                if (i < ProgressManager.Instance.CurrentTeam.Count)
                {
                    MobElements[i].SetUp(ProgressManager.Instance.CurrentTeam[i]);
                }
            }
        }

        public bool CheckForSameHero(MobData data)
        {
            foreach (var mobElement in MobElements)
            {
                if (mobElement.MData == data)
                {
                    return true;
                }
            }
            
            return false;       
        }
    }
}
