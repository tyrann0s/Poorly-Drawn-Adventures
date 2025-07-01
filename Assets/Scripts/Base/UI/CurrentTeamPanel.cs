using System;
using System.Collections.Generic;
using Managers.Base;
using Mobs;
using UnityEngine;

namespace Base.UI
{
    public class CurrentTeamPanel : MonoBehaviour, IMobPanel
    {
        private List<MobElement> mobElements = new();

        private void Awake()
        {
            mobElements.AddRange(gameObject.GetComponentsInChildren<MobElement>());
            foreach (var mobElement in mobElements)
            {
                mobElement.gameObject.AddComponent<DraggedTarget>();   
            }
        }

        public void UpdateTeamPanel()
        {
            for (int i = 0; i < mobElements.Count; i++)
            {
                if (i < ProgressManager.Instance.CurrentTeam.Count)
                {
                    mobElements[i].SetUp(ProgressManager.Instance.CurrentTeam[i]);
                }
            }
        }

        public bool CheckForSameHero(MobData data)
        {
            foreach (var mobElement in mobElements)
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
