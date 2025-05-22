using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CardPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject cardPrefab;

    [SerializeField]
    private List<Transform> spawnPositions = new List<Transform>();

    [SerializeField]
    private int ignoreLayer, normalLayer;

    public List<GameObject> Cards { get; private set; } = new List<GameObject>();

    private Vector3 originTransform;
    private Vector3 originScale = Vector3.one;

    public bool CardChangeMode { get; set; } = false;
    public int ChangeIndex { get; private set; } = 0;
    private int changesMadeThisRound = 0;
    private const int MAX_CHANGES_PER_ROUND = 1; // Максимальное количество изменений за раунд

    private UIManager uiManager;
    private GameManager gameManager;

    private void Start()
    {
        if (cardPrefab == null)
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

        uiManager = FindFirstObjectByType<UIManager>();
        gameManager = FindFirstObjectByType<GameManager>();

        if (uiManager == null)
        {
            Debug.LogError("UIManager not found!");
        }

        if (gameManager == null)
        {
            Debug.LogError("GameManager not found!");
        }
    }

    public void GenereteCards()
    {
        for (int i = 0; i < spawnPositions.Count; i++)
        {
            if (Cards[i] == null)
            {
                GameObject go = Instantiate(cardPrefab, spawnPositions[i]);
                Cards[i] = go;
                go.GetComponent<Card>().ParentCardPanel = this;
            }
        }
    }

    public void DisableInteraction()
    {
        foreach (GameObject card in Cards)
        {
            if (card != null) card.layer = ignoreLayer;
        }
        transform.position = originTransform;
    }

    public void EnableInteraction()
    {
        foreach (GameObject card in Cards)
        {
            if (card != null) card.layer = normalLayer;
        }
        transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
    }

    public void DeleteCards()
    {
        List<GameObject> cardsToDelete = new List<GameObject>(); 
        foreach (GameObject card in Cards)
        {
            if (card != null)
            {
                Card currentCard = card.GetComponent<Card>();

                if (currentCard.IsPicked || currentCard.IsForChange)
                {
                    cardsToDelete.Add(card);
                    Destroy(card);
                }
            }
        }

        foreach (GameObject card in cardsToDelete)
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
        uiManager.ShowChangeCardsButton();
    }

    public void StopChangeMode()
    {
        CardChangeMode = false;
        ChangeIndex = 0;
        DisableInteraction();
        uiManager.HideConfirmChangeButton();
        uiManager.HideChangeCardsButton();
    }

    public void IncreaseCardsForChange()
    {
        if (ChangeIndex < 2)
        {
            ChangeIndex++;
            uiManager.ShowConfirmChangeButton();
        }
    }

    public void DecreaseCardsForChange()
    {
        if (ChangeIndex > 0)
        {
            ChangeIndex--;
            if (ChangeIndex == 0)
            {
                uiManager.HideConfirmChangeButton();
            }
        }
    }

    public bool CanChangeCards()
    {
        return changesMadeThisRound < MAX_CHANGES_PER_ROUND;
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
        uiManager.HideConfirmChangeButton();
        uiManager.HideChangeCardsButton();
        DisableInteraction();
        ResetCardState();
        changesMadeThisRound++;
        
        // Сбрасываем ControlLock и ChangeCardMode после изменения карт
        if (gameManager != null)
        {
            gameManager.ControlLock = false;
            gameManager.ChangeCardMode = false;
        }
    }

    public void ResetRound()
    {
        changesMadeThisRound = 0;
        CardChangeMode = false;
        ChangeIndex = 0;
        DisableInteraction();
        uiManager.ShowChangeCardsButton();
    }

    private void ResetCardState()
    {
        foreach (GameObject card in Cards)
        {
            if (card != null)
            {
                Card cardComponent = card.GetComponent<Card>();
                if (cardComponent != null)
                {
                    cardComponent.IsPicked = false;
                    cardComponent.IsForChange = false;
                    cardComponent.transform.localScale = Vector3.one; // Возвращаем оригинальный размер
                    cardComponent.transform.position = card.transform.parent.position; // Возвращаем оригинальную позицию
                }
            }
        }
    }
}
