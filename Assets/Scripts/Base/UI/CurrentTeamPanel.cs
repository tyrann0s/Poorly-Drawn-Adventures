using System;
using System.Collections.Generic;
using Managers.Base;
using UnityEngine;

namespace Base.UI
{
    public class CurrentTeamPanel : MonoBehaviour
    {
        private List<MobElement> mobElements = new();

        private void Awake()
        {
            mobElements.AddRange(gameObject.GetComponentsInChildren<MobElement>());
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
    }
}
