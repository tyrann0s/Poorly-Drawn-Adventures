using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Managers;

namespace Cards
{
    public class CardPanel : MonoBehaviour
    {
        private static CardPanel instance;
        public static CardPanel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindFirstObjectByType<CardPanel>();
                    if (instance == null)
                    {
                        GameObject go = new GameObject("CardPanel");
                        instance = go.AddComponent<CardPanel>();
                    }
                }
                return instance;
            }
        }
        
        [SerializeField]
        private GameObject cardPrefab;

        [SerializeField]
        private List<Transform> spawnPositions = new List<Transform>();

        [SerializeField]
        private int ignoreLayer, normalLayer;
    
        [SerializeField]
        private CardCombination combinationSystem;
        public ElementCombo CurrentCombination { get; private set; }

        public List<Card> Cards { get; private set; } = new List<Card>();

        private Vector3 originTransform;

        public bool CardChangeMode { get; set; }
        public int ChangeIndex { get; private set; }
        private int changesMadeThisRound;
        private const int MaxChangesPerRound = 1; // Максимальное количество изменений за раунд

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
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

        public void GenereteCards()
        {
            for (int i = 0; i < spawnPositions.Count; i++)
            {
                if (Cards[i] == null)
                {
                    GameObject go = Instantiate(cardPrefab, spawnPositions[i]);
                    Cards[i] = go.GetComponent<Card>();
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
            List<Card> cardsToDelete = new List<Card>(); 
            foreach (Card card in Cards)
            {
                if (card)
                {
                    if (card.IsPicked || card.IsForChange)
                    {
                        cardsToDelete.Add(card);
                        Destroy(card.gameObject);
                    }
                }
            }

            foreach (Card card in cardsToDelete)
            {
                Cards[Cards.IndexOf(card)] = null;
            }
        }

        public void StartChangeMode()
        {
            if (!CanChangeCards())
            {
                Debug.LogWarning("Cannot start change mode - max changes per round reached!");
                return;
            }

            CardChangeMode = true;
            EnableInteraction();
            UIManager.Instance.ShowChangeCardsButton();
        }

        public void StopChangeMode()
        {
            CardChangeMode = false;
            ChangeIndex = 0;
            DisableInteraction();
            UIManager.Instance.HideConfirmChangeButton();
            UIManager.Instance.HideChangeCardsButton();
        }

        public void IncreaseCardsForChange()
        {
            if (ChangeIndex < 2)
            {
                ChangeIndex++;
                UIManager.Instance.ShowConfirmChangeButton();
            }
        }

        public void DecreaseCardsForChange()
        {
            if (ChangeIndex > 0)
            {
                ChangeIndex--;
                if (ChangeIndex == 0)
                {
                    UIManager.Instance.HideConfirmChangeButton();
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
            GenereteCards();
            UIManager.Instance.HideConfirmChangeButton();
            UIManager.Instance.HideChangeCardsButton();
            DisableInteraction();
            ResetCardState();
            changesMadeThisRound++;
        
            // Сбрасываем ControlLock и  после изменения карт
            GameManager.Instance.ControlLock = false;
        }

        public void ResetRound()
        {
            changesMadeThisRound = 0;
            CardChangeMode = false;
            ChangeIndex = 0;
            DisableInteraction();
            UIManager.Instance.ShowChangeCardsButton();
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

            UIManager.Instance.ShowCombination(CurrentCombination);
        }

        public ElementCombo GetCombo()
        {
            ElementCombo combo = CurrentCombination;
            CurrentCombination = null;
            return combo;
        }
    }
}