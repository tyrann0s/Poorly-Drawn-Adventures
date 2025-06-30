using Managers.Base;
using Mobs;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Base.UI
{
    public class MobElement : MonoBehaviour, IDraggable
    {
        [SerializeField] private bool isTarget;
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private GameObject pricePanel;
        [SerializeField] private TextMeshProUGUI priceText;

        private Transform originalParent;
        private Vector3 originalPosition;

        public MobData MData { get; set; }

        public void SetUp(MobData mobData)
        {
            MData = mobData;
            icon.sprite = MData.mobIcon;
            nameText.text = MData.MobName;
            priceText.text = MData.HireCost.ToString();
        }

        public Transform GetOriginalParent()
        {
            return originalParent;
        }

        public Vector3 GetOriginalPosition()
        {
            return originalPosition;
        }

        public void SetOriginalParent()
        {
            originalParent = transform.parent;
        }

        public void SetOriginalPosition()
        {
            originalPosition = transform.position;
        }

        public void ApplyDrag(IDraggable draggedElement)
        {
            // Копируем данные из перетаскиваемого объекта в текущий
            if (draggedElement is MobElement element)
            {
                if (element.MData.HireCost <= ProgressManager.Instance.Coins)
                {
                    BaseManager.Instance.SpendCoins(element.MData.HireCost);
                    
                    SetUp(element.MData);

                    // Возвращаем перетаскиваемый объект на исходную позицию
                    element.transform.SetParent(element.GetOriginalParent(), true);
                    element.transform.position = element.GetOriginalPosition();
                }
                else
                {
                    Debug.Log("NOT ENOUGH COINS");
                }
            }
        }
        
        public void ShowPricePanel()
        {
            pricePanel.SetActive(true);
        }
    }
}