using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Mobs
{
    public class MobButtons : MonoBehaviour
    {
        [SerializeField]
        private MobButton skipTurnbutton, defenseButton, attack1Button, attack2Button;

        [SerializeField]
        private float animationSpeed;

        [SerializeField]
        private float scaleAmount;
        public float ScaleAmount => scaleAmount;

        private List<MobButton> buttons = new List<MobButton>();
        private Mob mob;

        private void Start()
        {
            mob = GetComponentInParent<Mob>();

            buttons.Add(skipTurnbutton);
            buttons.Add(defenseButton);
            buttons.Add(attack1Button);
            buttons.Add(attack2Button);

            foreach (MobButton button in buttons) { button.gameObject.SetActive(false); }
        }

        public void ShowButtons()
        {
            skipTurnbutton.SetActive(true, mob);

            if (mob.MobStamina < mob.MobData.DefenseCost) defenseButton.SetActive(false, mob);
            else defenseButton.SetActive(true, mob);

            if (mob.MobStamina < mob.MobData.Attack1Cost) attack1Button.SetActive(false, mob);
            else attack1Button.SetActive(true, mob);

            if (mob.MobStamina < mob.MobData.Attack2Cost) attack2Button.SetActive(false, mob);
            else attack2Button.SetActive(true, mob);

            foreach (MobButton button in buttons) { button.gameObject.SetActive(true); }
            DOTween.Sequence()
                .Append(skipTurnbutton.transform.DOLocalMove(skipTurnbutton.Destination(), animationSpeed))
                .Join(skipTurnbutton.transform.DOScale(.3f, animationSpeed))
                .Append(defenseButton.transform.DOLocalMove(defenseButton.Destination(), animationSpeed))
                .Join(defenseButton.transform.DOScale(.3f, animationSpeed))
                .Append(attack1Button.transform.DOLocalMove(attack1Button.Destination(), animationSpeed))
                .Join(attack1Button.transform.DOScale(.3f, animationSpeed))
                .Append(attack2Button.transform.DOLocalMove(attack2Button.Destination(), animationSpeed))
                .Join(attack2Button.transform.DOScale(.3f, animationSpeed));

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
    
        private void OnDestroy()
        {
            // Останавливает все твины, связанные с этим объектом
            DOTween.Kill(transform);
        }
    }
}