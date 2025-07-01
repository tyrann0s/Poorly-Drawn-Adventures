using System;
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
        
        private IMobPanel mobPanel;

        public MobData MData { get; set; }

        private void Awake()
        {
            mobPanel = GetComponentInParent<IMobPanel>();
        }

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
                if (element.MData == MData)
                {
                    Debug.Log("CAN'T HIRE SAME MOB");
                    return;
                }

                if (element.MData.Type == MobType.Hero && mobPanel.CheckForSameHero(element.MData))
                {
                    Debug.Log("ALREADY HAVE THIS MOB");
                    return;
                }
                
                if (element.MData.HireCost <= ProgressManager.Instance.Coins)
                {
                    BaseManager.Instance.SpendCoins(element.MData.HireCost);
                    
                    SetUp(element.MData);

                    switch (element.MData.Type)
                    {
                        case MobType.Mob:
                            element.transform.SetParent(element.GetOriginalParent(), true);
                            element.transform.position = element.GetOriginalPosition();
                            break;
                        case MobType.Hero:
                            Destroy(element.gameObject);
                            break;
                        case MobType.Boss:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
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