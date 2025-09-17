using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using Managers;
using TMPro;

namespace Cards
{
    public class CardPanel : MonoBehaviour, IManager
    {
        [SerializeField]
        private GameObject cardPrefab;

        [SerializeField]
        private List<Transform> spawnPositions = new List<Transform>();

        [SerializeField] private Transform cardsBlock;
        [SerializeField] private GameObject buttonsPanel, currentComboPanel;
        [SerializeField] private TextMeshProUGUI currentComboText;
    
        [SerializeField]
        private CardCombination combinationSystem;
        
        [SerializeField] private GameObject bkgBack, bkgFront;
        private RectTransform bkgBackRect, bkgFrontRect;
        
        public ElementCombo CurrentCombination { get; private set; }

        public List<Card> Cards { get; private set; } = new();

        public bool CardChangeMode { get; set; }

        public int ChangeIndex { get; set; }
        private int changesMadeThisRound;
        private const int MaxChangesPerRound = 1; // Максимальное количество изменений за раунд

        private List<(int rank, ElementType element)> lastDeletedCards = new();

        private int cardsToAnimate;

        [SerializeField] private GameObject cardTrailPrefab;
        
        public void Initialize()
        {
            if (!cardPrefab)
            {
                Debug.LogError("Card prefab is not assigned!");
                return;
            }

            if (spawnPositions == null || spawnPositions.Count == 0)
            {
                Debug.LogError("No spawn positions found!");
                return;
            }

            for (int i = 0; i < spawnPositions.Count; i++)
            {
                Cards.Add(null);
            }

            bkgBackRect = bkgBack.GetComponent<RectTransform>();
            bkgFrontRect = bkgFront.GetComponent<RectTransform>();
        }

        public void GenerateCards(bool isChanging)
        {
            cardsToAnimate = 0;
            
            for (int i = 0; i < spawnPositions.Count; i++)
            {
                if (!Cards[i])
                {
                    GameObject go = Instantiate(cardPrefab, spawnPositions[i]);
                    Cards[i] = go.GetComponent<Card>();
                    
                    if (isChanging)
                    {
                        // Генерируем карты до тех пор, пока не получим уникальную
                        while (true)
                        {
                            Cards[i].InitializeCard();
                            
                            bool isDuplicate = false;
                            
                            // Проверяем по сохраненной информации об удаленных картах
                            foreach (var deletedCardInfo in lastDeletedCards)
                            {
                                if (deletedCardInfo.rank == Cards[i].GetRank() || 
                                    deletedCardInfo.element == Cards[i].GetElement())
                                {
                                    isDuplicate = true;
                                    break;
                                }
                            }
                            
                            // Если карта уникальна, выходим из цикла генерации
                            if (!isDuplicate)
                            {
                                Cards[i].enabled = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        Cards[i].InitializeCard();
                        cardsToAnimate++;
                    }
                    
                    Cards[i].ParentCardPanel = this;
                }
            }

            StartCoroutine(DealingCardsAnimation());
        }

        public void DisableInteraction()
        {
            cardsBlock.DOScale(0, .5f).SetEase(Ease.InOutBack);;
            foreach (Card card in Cards)
            {
                card?.HideCard();
            }
            HideBKG();
        }

        public void EnableInteraction()
        {
            bool cardEnabled = false;
            
            foreach (Card card in Cards)
            {
                if (card) cardEnabled = true;
            }
            
            if (!cardEnabled) return;

            cardsBlock.DOScale(1, .5f).SetEase(Ease.InOutBack);;
            foreach (Card card in Cards)
            {
                card?.ShowCard();
            }
            ResetCardState();
            ShowBKG();
        }

        public void DeleteCards()
        {
            // Временно сохраняем информацию о картах перед удалением
            var cardsInfo = new List<(int rank, ElementType element)>();
            
            for (int i = 0; i < Cards.Count; i++)
            {
                if (Cards[i] && (Cards[i].IsPicked || Cards[i].IsForChange))
                {
                    // Сохраняем только необходимую информацию, а не ссылки на объекты
                    cardsInfo.Add((Cards[i].GetRank(), Cards[i].GetElement()));
                    
                    if (Cards[i].IsForChange) changesMadeThisRound++;
                    
                    Cards[i].transform.DOKill(true);
                    Destroy(Cards[i].gameObject);
                    Cards[i] = null;
                }
            }
            
            // Теперь используем cardsInfo вместо cardsToDelete
            // Сохраняем информацию для генерации новых карт
            lastDeletedCards = cardsInfo;
        }

        public void StartChangeMode()
        {
            if (!CanChangeCards())
            {
                Debug.LogWarning("Cannot start change mode - max changes per round reached!");
                return;
            }

            ChangeIndex = 0;
            ServiceLocator.Get<UIManager>().EnterChangeCards();
            
            if (CardChangeMode)
            {
                CardChangeMode = false;
                DisableInteraction();
            }
            else
            {
                CardChangeMode = true;
                EnableInteraction();
            }
        }

        public void IncreaseCardsForChange()
        {
            if (ChangeIndex <= 2)
            {
                ChangeIndex++;
                ServiceLocator.Get<UIManager>().SetConfirmChangeButton(true);
            }
        }

        public void DecreaseCardsForChange()
        {
            if (ChangeIndex > 0)
            {
                ChangeIndex--;
                if (ChangeIndex == 0)
                {
                    ServiceLocator.Get<UIManager>().SetConfirmChangeButton(false);
                }
            }
        }

        public bool CanChangeCards()
        {
            return changesMadeThisRound <= MaxChangesPerRound;
        }

        public void ChangeCards()
        {
            if (!CanChangeCards())
            {
                Debug.Log("Cannot change cards more than once per round!");
                return;
            }

            DeleteCards();
            GenerateCards(true);
            
            ServiceLocator.Get<UIManager>().SetConfirmChangeButton(false);

            if (!CanChangeCards())
            {
                ServiceLocator.Get<UIManager>().HideConfirmChangeButton();
            }
        }

        public void ResetRound()
        {
            changesMadeThisRound = 0;
            CardChangeMode = false;
            ChangeIndex = 0;
            ServiceLocator.Get<UIManager>().PrepareChangeCards();
        }

        private void ResetCardState()
        {
            foreach (Card card in Cards.Where(c => c != null))
            {
                card.IsPicked = false;
                card.IsForChange = false;
            }
        }

        public void CheckForCombination()
        {
            var pickedCards = Cards.Where(card => card != null && card.IsPicked).ToList();
            CurrentCombination = combinationSystem.CheckCombination(pickedCards);

            if (ProgressManager.Instance.RecordsCombo.Contains(CurrentCombination)) ShowCurrentCombo();
            else HideCurrentCombo();
        }

        public ElementCombo GetCombo()
        {
            ElementCombo combo = CurrentCombination;
            CurrentCombination = null;
            return combo;
        }
        
        private void OnDestroy()
        {
            // Убедитесь, что все твины полностью очищены
            if (cardsBlock != null)
            {
                cardsBlock.DOKill(true); // true для завершения всех твинов
            }
            
            // Очистка твинов для фоновых элементов
            if (bkgBackRect != null)
            {
                bkgBackRect.DOKill(true);
            }
            if (bkgFrontRect != null)
            {
                bkgFrontRect.DOKill(true);
            }
            
            // Очищаем списки
            lastDeletedCards?.Clear();
            Cards?.Clear();
        }

        public void ShowButtons()
        {
            buttonsPanel.SetActive(true);
        }

        public void HideButtons()
        {
            buttonsPanel.SetActive(false);
        }
        
        public void ShowCurrentCombo()
        {
            currentComboPanel.SetActive(true);
            currentComboText.text = CurrentCombination.comboName;
        }
        
        public void HideCurrentCombo()
        {
            currentComboPanel.SetActive(false);
        }

        private void ShowBKG()
        {
            bkgBackRect.DOAnchorPos3DY(-380, .5f).SetEase(Ease.InOutBack);
            bkgFrontRect.DOAnchorPos3DY(-125,.5f).SetEase(Ease.InOutBack);
        }

        private void HideBKG()
        {
            bkgBackRect.DOAnchorPos3DY(-580, .5f).SetEase(Ease.InOutBack);
            bkgFrontRect.DOAnchorPos3DY(-180, .5f).SetEase(Ease.InOutBack);
        }

        private IEnumerator DealingCardsAnimation()
        {
            for (int i = 0; i < cardsToAnimate; i++)
            {
                var trail = Instantiate(cardTrailPrefab);
                float randomPos = Random.Range(-1.5f, 1.5f);
                trail.transform.position = new Vector3(randomPos, randomPos, 0);
                trail.transform.DOMoveY(-5, .2f).SetEase(Ease.InOutBack);
                
                yield return new WaitForSeconds(0.2f);
                
                Destroy(trail);
                
                bkgBackRect.DOShakePosition(0.1f, 10, 10, 90);
                bkgFrontRect.DOShakePosition(0.1f, 10, 10, 90);
            }
            
            ServiceLocator.Get<GameManager>().PrepPhaseReady();
        }
    }
}