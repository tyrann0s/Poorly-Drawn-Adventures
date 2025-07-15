using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI
{
    public class NotificationPanel : MonoBehaviour
    {
        private TextMeshProUGUI notificationText;

        private void Awake()
        {
            notificationText = GetComponentInChildren<TextMeshProUGUI>(true);
        }
        
        public void ShowNotification(string text)
        {
            notificationText.text = text;
            
            DOTween.Sequence()
                .SetDelay(5f)
                .Append(transform.DOScale(0f, .3f))
                .OnComplete(HidePanel);
        }

        private void HidePanel()
        {
            gameObject.SetActive(false);
        }
    }
}
