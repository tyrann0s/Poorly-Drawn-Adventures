using UnityEngine;

namespace Mobs
{
    public class AnimationEvent : MonoBehaviour
    {
        private MobActions mobAction;

        private void Start()
        {
            mobAction = GetComponentInParent<MobActions>();
        }

        public void Attack()
        {
            mobAction.MakeDamage();
        }

        public void ActiveSkill()
        {
            mobAction.MakeActiveSkill();
        }
    
        public void PassiveSkill()
        {
        
        }
    }
}
