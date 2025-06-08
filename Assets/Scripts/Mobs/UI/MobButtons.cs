using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Mobs
{
    public class MobButtons : MonoBehaviour
    {
        [SerializeField]
        private MobButton skipTurnButton, defenseButton, attackButton, skillButton;

        [SerializeField]
        private float animationSpeed;

        [SerializeField]
        private float scaleAmount;
        public float ScaleAmount => scaleAmount;

        private List<MobButton> buttons = new();
        private Mob mob;

        private void Start()
        {
            mob = GetComponentInParent<Mob>();

            buttons.Add(skipTurnButton);
            buttons.Add(defenseButton);
            buttons.Add(attackButton);
            buttons.Add(skillButton);

            foreach (MobButton button in buttons) { button.gameObject.SetActive(false); }
        }

        public void ShowButtons()
        {
            skipTurnButton.SetActive(true, mob);

            if (mob.MobStamina < mob.MobData.DefenseCost) defenseButton.SetActive(false, mob);
            else defenseButton.SetActive(true, mob);

            if (mob.MobStamina < mob.MobData.AttackCost) attackButton.SetActive(false, mob);
            else attackButton.SetActive(true, mob);

            if (mob.MobStamina < mob.MobData.SkillCost) skillButton.SetActive(false, mob);
            else skillButton.SetActive(true, mob);

            foreach (MobButton button in buttons) { button.gameObject.SetActive(true); }
            DOTween.Sequence()
                .Append(skipTurnButton.transform.DOLocalMove(skipTurnButton.Destination(), animationSpeed))
                .Join(skipTurnButton.transform.DOScale(.3f, animationSpeed))
                .Append(defenseButton.transform.DOLocalMove(defenseButton.Destination(), animationSpeed))
                .Join(defenseButton.transform.DOScale(.3f, animationSpeed))
                .Append(attackButton.transform.DOLocalMove(attackButton.Destination(), animationSpeed))
                .Join(attackButton.transform.DOScale(.3f, animationSpeed))
                .Append(skillButton.transform.DOLocalMove(skillButton.Destination(), animationSpeed))
                .Join(skillButton.transform.DOScale(.3f, animationSpeed));

        }

        public void HideButtons()
        {
            if (buttons == null) return;

            // Удаляем уничтоженные кнопки из списка
            buttons.RemoveAll(button => button == null);

            foreach (var button in buttons)
            {
                if (button && button.transform)
                {
                    // Выполняем действия с кнопкой
                    button.transform.localScale = Vector3.zero;
                    // или
                    button.gameObject.SetActive(false);
                }
            }
        }

        private void DeactivateButtons()
        {
            foreach (MobButton button in buttons) { button.gameObject.SetActive(false); }
        }

        public void RenameSkillButton(string value)
        {
            skillButton.ChangeText(value);
        }
    
        private void OnDestroy()
        {
            // Останавливает все твины, связанные с этим объектом
            DOTween.Kill(transform);
        }
    }
}