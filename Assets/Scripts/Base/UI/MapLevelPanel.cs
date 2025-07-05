using DG.Tweening;
using Mobs;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace Hub.UI
{
    public class MapLevelPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text description;
        [SerializeField] private TMP_Text goldReward;
        [SerializeField] private TMP_Text mobReward, heroReward;
        [SerializeField] private Image mobRewardIcon, heroRewardIcon;

        public void SetupPanel(string description, string goldReward, MobData mob, MobData hero)
        {
            this.description.text = description;
            this.goldReward.text = goldReward;
            mobReward.text = mob.MobName;
            mobRewardIcon.sprite = mob.mobIcon;
            heroReward.text = hero.MobName;
            heroRewardIcon.sprite = hero.mobIcon;
        }

        public void ShowPanel()
        {
            DOTween.Sequence()
                .Append(transform.DOScale(1, .2f))
                .SetEase(Ease.InOutQuint);
        }
        
        public void HidePanel()
        {
            DOTween.Sequence()
                .Append(transform.DOScale(0, .2f))
                .SetEase(Ease.InOutQuint);
        }
    }
}
