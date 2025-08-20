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
            skillButton.DescriptionText = mob.MobData.ActiveSkill.Description;
        }

        public void ShowButtons()
        {
            skipTurnButton.SetButtonText("Skip Turn");
            defenseButton.SetButtonText($"Defense ({mob.MobData.DefenseCost + mob.MobStatusEffects.GetStaminaChange()} SP)");
            attackButton.SetButtonText($"Attack ({mob.MobData.AttackCost + mob.MobStatusEffects.GetStaminaChange()} SP)");
            skillButton.SetButtonText($"{mob.MobData.ActiveSkill.SkillName} ({mob.MobData.ActiveSkill.Cost + mob.MobStatusEffects.GetStaminaChange()} SP)");
            
            defenseButton.SetActive(mob.CheckStamina(mob.MobData.DefenseCost));
            attackButton.SetActive(mob.CheckStamina(mob.MobData.AttackCost));
            skillButton.SetActive(mob.CheckStamina(mob.MobData.SkillCost));
            
            transform.DOScale(originalScale, .3f);
        }

        public void HideButtons()
        {
            transform.DOScale(Vector3.zero, .3f);
        }
    
        private void OnDestroy()
        {
            // Останавливает все твины, связанные с этим объектом
            DOTween.Kill(transform);
        }
    }
}