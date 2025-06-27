using System.Collections.Generic;
using UnityEngine;

namespace Base.UI
{
    public class MobListPanel : MonoBehaviour
    {
        private List<MobElement> mobElements = new();

        private void Awake()
        {
            mobElements.AddRange(gameObject.GetComponentsInChildren<MobElement>());
        }
    }
}
