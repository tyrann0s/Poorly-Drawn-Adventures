using UnityEngine;

namespace Mobs
{
    public class MobComponent : MonoBehaviour
    {
        protected Mob ParentMob { get; private set; }

        protected virtual void Awake()
        {
            ParentMob = GetComponent<Mob>();
        }
    }
}
