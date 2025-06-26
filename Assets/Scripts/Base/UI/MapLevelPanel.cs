using DG.Tweening;
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
        [SerializeField] private TMP_Text mobReward;
        [SerializeField] private Image mobRewardIcon;

        public void SetupPanel(string description, string goldReward, string mobReward, Sprite mobRewardIcon)
        {
            this.description.text = description;
            this.goldReward.text = goldReward;
            this.mobReward.text = mobReward;
            this.mobRewardIcon.sprite = mobRewardIcon;
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
