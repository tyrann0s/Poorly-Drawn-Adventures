using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Managers;
using Managers.Base;

namespace Cards
{
    public class CardPanel : MonoBehaviour, IManager
    {
        [SerializeField]
        private GameObject cardPrefab;

        [SerializeField]
        private List<Transform> spawnPositions = new List<Transform>();

        [SerializeField]
        private int ignoreLayer, normalLayer;
    
        [SerializeField]
        private CardCombination combinationSystem;
        public ElementCombo CurrentCombination { get; private set; }

        public List<Card> Cards { get; private set; } = new();

        private Vector3 originTransform;

        public bool CardChangeMode { get; set; }
        private List<Card> cardsToDelete = new(); 
        public int ChangeIndex { get; private set; }
        private int changesMadeThisRound;
        private const int MaxChangesPerRound = 1; // Максимальное количество изменений за раунд
        
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

            originTransform = transform.position;
        }

        public void GenereteCards(bool isChanging)
        {
            for (int i = 0; i < spawnPositions.Count; i++)
            {
                if (Cards[i] == null)
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
                            
                            // Проверяем, есть ли уже такая карта в списке удаляемых
                            foreach (Card cardToDelete in cardsToDelete)
                            {
                                if (cardToDelete.GetRank() == Cards[i].GetRank() || 
                                    cardToDelete.GetElement() == Cards[i].GetElement())
                                {
                                    isDuplicate = true;
                                }
                            }
                            
                            // Если карта уникальна, выходим из цикла генерации
                            if (!isDuplicate)
                                break;
                        }
                    }
                    else
                    {
                        Cards[i].InitializeCard();
                    }

                    Cards[i].ParentCardPanel = this;
                }
            }
        }

        public void DisableInteraction()
        {
            foreach (Card card in Cards)
            {
                if (card) card.gameObject.layer = ignoreLayer;
            }
            transform.position = originTransform;
        }

        public void EnableInteraction()
        {
            foreach (Card card in Cards)
            {
                if (card) card.gameObject.layer = normalLayer;
            }
            transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        }

        public void DeleteCards()
        {
            // Очищаем список перед заполнением
            cardsToDelete.Clear();
            
            for (int i = 0; i < Cards.Count; i++)
            {
                if (Cards[i] != null && (Cards[i].IsPicked || Cards[i].IsForChange))
                {
                    // Добавляем карту в список для удаления ДО уничтожения объекта
                    cardsToDelete.Add(Cards[i]);
                    Destroy(Cards[i].gameObject);
                    Cards[i] = null;
                }
            }
        }

        public void StartChangeMode()
        {
            if (!CanChangeCards())
            {
                Debug.LogWarning("Cannot start change mode - max changes per round reached!");
                return;
            }

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

        public void StopChangeMode()
        {
            CardChangeMode = false;
            ChangeIndex = 0;
            DisableInteraction();
            ServiceLocator.Get<UIManager>().ExitChangeCards();
        }

        public void IncreaseCardsForChange()
        {
            if (ChangeIndex < 2)
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
            return changesMadeThisRound < MaxChangesPerRound;
        }

        public void ChangeCards()
        {
            if (!CanChangeCards())
            {
                Debug.LogWarning("Cannot change cards more than once per round!");
                return;
            }

            DeleteCards();
            GenereteCards(true);
            
            ServiceLocator.Get<UIManager>().ExitChangeCards();
            DisableInteraction();
            ResetCardState();
            changesMadeThisRound++;
        
            // Сбрасываем ControlLock и  после изменения карт
            ServiceLocator.Get<GameManager>().ControlLock = false;
        }

        public void ResetRound()
        {
            changesMadeThisRound = 0;
            CardChangeMode = false;
            ChangeIndex = 0;
            DisableInteraction();
            ServiceLocator.Get<UIManager>().PrepareChangeCards();
        }

        private void ResetCardState()
        {
            foreach (Card card in Cards)
            {
                if (card != null)
                {
                    if (card.IsPicked || card.IsForChange)
                    {
                        card.IsPicked = false;
                        card.IsForChange = false;
                        card.transform.localScale = Vector3.one;
                        card.transform.position = card.transform.parent.position;
                    }
                }
            }
        }

        public void CheckForCombination()
        {
            var pickedCards = Cards.Where(card => card != null && card.IsPicked).ToList();
            CurrentCombination = combinationSystem.CheckCombination(pickedCards);

            if (ProgressManager.Instance.RecordsCombo.Contains(CurrentCombination)) ServiceLocator.Get<UIManager>().ShowCombination(CurrentCombination.comboName);
            else ServiceLocator.Get<UIManager>().ShowCombination("");
        }

        public ElementCombo GetCombo()
        {
            ElementCombo combo = CurrentCombination;
            CurrentCombination = null;
            return combo;
        }
    }
}