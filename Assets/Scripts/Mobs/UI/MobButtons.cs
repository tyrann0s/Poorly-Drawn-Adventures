using DG.Tweening;
using Mobs.Skills;
using UnityEngine;

namespace Mobs
{
    public class MobButtons : MonoBehaviour
    {
        [SerializeField] private MobButton skipTurnButton, defenseButton, attackButton, skillButton;

        private Vector3 originalScale;
        
        private Mob mob;

        private void Start()
        {
            originalScale = transform.localScale;
            transform.localScale = Vector3.zero;
            
            mob = GetComponentInParent<Mob>();
            skipTurnButton.DescriptionText = "Skip Turn and restore stamina";
            defenseButton.DescriptionText = "Set shield on a mob";
            attackButton.DescriptionText = "Make melee damage to a mob";
        }

        public void ShowButtons()
        {
            defenseButton.SetActive(mob.CheckStamina(mob.MobData.DefenseCost));
            attackButton.SetActive(mob.CheckStamina(mob.MobData.AttackCost));
            skillButton.SetActive(mob.CheckStamina(mob.MobData.SkillCost));
            
            transform.DOScale(originalScale, .3f);
        }

        public void HideButtons()
        {
            transform.DOScale(Vector3.zero, .3f);
        }

        public void PrepareSkillButton(ActiveSkill activeSkill)
        {
            skillButton.SetButtonText(activeSkill.SkillName + $" ({activeSkill.Cost} SP)");
            skillButton.DescriptionText = activeSkill.Description;
        }
    
        private void OnDestroy()
        {
            // Останавливает все твины, связанные с этим объектом
            DOTween.Kill(transform);
        }
    }
}