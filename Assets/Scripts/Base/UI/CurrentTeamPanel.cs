using System;
using System.Collections.Generic;
using Managers.Base;
using UnityEngine;

namespace Base.UI
{
    public class CurrentTeamPanel : MonoBehaviour
    {
        private List<MobElement> mobElements = new();

        private void Start()
        {
            mobElements.AddRange(gameObject.GetComponentsInChildren<MobElement>());
        }

        public void UpdateTeamPanel()
        {
            for (int i = 0; i < mobElements.Count; i++)
            {
                if (i < BaseManager.Instance.CurrentTeam.Count)
                {
                    mobElements[i].SetUp(BaseManager.Instance.CurrentTeam[i].mobIcon, BaseManager.Instance.CurrentTeam[i].MobName);
                }
            }
        }
    }
}
