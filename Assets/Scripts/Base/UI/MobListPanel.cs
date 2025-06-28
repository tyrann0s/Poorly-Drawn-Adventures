using System.Collections.Generic;
using Mobs;
using UnityEngine;

namespace Base.UI
{
    public class MobListPanel : MonoBehaviour
    {
        [SerializeField] private GameObject mobElementPrefab;

        public void UpdateMobPanel(List<MobData> mobList)
        {
            foreach (var mobData in mobList)
            {
                AddMob(mobData);
            }
        }

        private void AddMob(MobData mobData)
        {
            var element = Instantiate(mobElementPrefab, transform);
            element.GetComponent<MobElement>().SetUp(mobData);
        }
    }
}
