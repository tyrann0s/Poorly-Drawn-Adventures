using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Mobs
{
    public class MobButtons : MonoBehaviour
    {
        private Mob mob;

        private void Start()
        {
            mob = GetComponentInParent<Mob>();
        }

        public void ShowButtons()
        {
            

        }

        public void HideButtons()
        {
            
        }

        public void RenameSkillButton(string value)
        {
            
        }
    
        private void OnDestroy()
        {
            // Останавливает все твины, связанные с этим объектом
            DOTween.Kill(transform);
        }
    }
}