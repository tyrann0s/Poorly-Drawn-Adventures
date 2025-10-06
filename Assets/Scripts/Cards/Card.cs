using DG.Tweening;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Cards
{
    public class Card : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private TMP_Text rankText;

        [SerializeField]
        private Image elementImage;

        private Element cardElement = new ();
        private int cardRank;

        private bool isHovering;
        public bool IsPicked { get; set; }
        public bool IsForChange { get; set; }

        public CardPanel ParentCardPanel { get; set; }

        private Vector3 originScale;
        
        private Material originalMaterial, outlineMaterial;
        private Image cardBackgroundImage;

        public void InitializeCard()
        {
            cardElement.CurrentElementType = GetRandomElementType();
            cardRank = Random.Range(1, 7);
            originScale = transform.localScale;
            
            Prepare();
        }

        public void InitializeCard(ElementType elementType, int rank)
        {
            cardElement.CurrentElementType = elementType;
            cardRank = rank;
            originScale = transform.localScale;
            
            Prepare();
        }

        private void Prepare()
        {
            rankText.text = cardRank.ToString();
            elementImage.sprite = ResourceManager.Instance.Icons.GetIcon(cardElement.CurrentElementType);
            enabled = false;
            
            cardBackgroundImage = GetComponent<Image>();
            originalMaterial = cardBackgroundImage.material;
            outlineMaterial = new Material(originalMaterial);
        }

        private ElementType GetRandomElementType()
        {
            const int firstElementIndex = (int)ElementType.Fire;
            const int lastElementIndex = (int)ElementType.Earth;
            
            // Генерируем случайное значение только среди стихийных элементов
            return (ElementType)Random.Range(firstElementIndex, lastElementIndex + 1);
        }
    
        public ElementType GetElement()
        {
            return cardElement.CurrentElementType;
        }


        public int GetRank()
        {
            return cardRank;
        }

        public void ShowCard()
        {
            HideOutline();
            transform.localScale = originScale;
            IsForChange = false;
            IsPicked = false;
            enabled = true;
        }
        
        public void HideCard()
        {
            enabled = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (ParentCardPanel.ItemCardForChange)
            {
                InitializeCard(ParentCardPanel.ItemCardForChange.CardElement, ParentCardPanel.ItemCardForChange.CardRank);
                ParentCardPanel.ItemCardForChange.UseComplete();
                Debug.Log(ParentCardPanel.ItemCardForChange);
                //ParentCardPanel.ItemCardForChange = null;
                ServiceLocator.Get<UIManager>().ExitChangeCards();
            }
            
            if (ParentCardPanel.CardChangeMode)
            {
                if (IsForChange)
                {
                    IsForChange = false;
                    transform.localScale = originScale;
                    ParentCardPanel.DecreaseCardsForChange();
                    HideOutline();
                }
                else if (ParentCardPanel.ChangeIndex < 2)
                {
                    IsForChange = true;
                    transform.localScale *= 1.1f;
                    ParentCardPanel.IncreaseCardsForChange();
                    ShowOutline();
                }
            }
            else
            {
                if (IsPicked)
                {
                    IsPicked = false;
                    transform.localScale = originScale;
                    HideOutline();
                }
                else
                {
                    IsPicked = true;
                    transform.localScale *= 1.1f;
                    ShowOutline();
                }

                ParentCardPanel.CheckForCombination();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!isHovering && ParentCardPanel.ChangeIndex < 2)
            {
                transform.DOScale(1.1f, .2f).SetEase(Ease.InOutQuint);
                
                isHovering = true;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (IsPicked || IsForChange) return;

            isHovering = false;
            transform.DOScale(1, .2f).SetEase(Ease.InOutQuint);
        }

        private void ShowOutline()
        {
            outlineMaterial.SetColor("_Outline_Color", Color.white);
            outlineMaterial.SetInt("OUTLINE_ON", 1);
            cardBackgroundImage.material = outlineMaterial;

        }
        
        public void HideOutline()
        {
            outlineMaterial.SetInt("OUTLINE_ON", 0);
            cardBackgroundImage.material = outlineMaterial;

        }
    }
}