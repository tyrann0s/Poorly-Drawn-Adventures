using System;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField]
    private TMP_Text rankText;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Sprite fireSprite, waterSprite, airSprite, earthSprite;

    private Element cardElement = new Element();
    private int cardRank;

    private bool isHovering = false;
    public bool IsPicked { get; set; } = false;
    public bool IsForChange { get; set; } = false;

    public CardPanel ParentCardPanel { get; set; }

    private Vector3 originTransform, originScale;

    private void Awake()
    {
        if (fireSprite == null || waterSprite == null || airSprite == null || earthSprite == null)
        {
            Debug.LogError("Missing card element sprites!");
            return;
        }

        cardElement.CurrentElementType = (ElementType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(ElementType)).Length);
        cardRank = UnityEngine.Random.Range(1, 7);
        originTransform = transform.position;
        originScale = transform.localScale;
    }

    private void Start()
    {
        if (rankText == null || spriteRenderer == null)
        {
            Debug.LogError("Missing required components on card!");
            return;
        }

        rankText.text = cardRank.ToString();

        switch (cardElement.CurrentElementType)
        {
            case ElementType.Fire:
                spriteRenderer.sprite = fireSprite;
                break;
            case ElementType.Water:
                spriteRenderer.sprite = waterSprite;
                break;
            case ElementType.Air:
                spriteRenderer.sprite = airSprite;
                break;
            case ElementType.Earth:
                spriteRenderer.sprite = earthSprite;
                break;
        }
    }

    private void OnMouseEnter()
    {
        if (!isHovering && ParentCardPanel.ChangeIndex < 2)
        {
            originTransform = transform.position;
            transform.position = new Vector3(transform.position.x, transform.position.y + .25f, transform.position.z);
            isHovering = true;
        }
    }


    private void OnMouseExit()
    {
        if (IsPicked || IsForChange) return;

        isHovering = false;
        transform.position = originTransform;
    }

    private void OnMouseDown()
    {
        if (ParentCardPanel.CardChangeMode)
        {
            if (IsForChange)
            {
                IsForChange = false;
                transform.localScale = originScale;
                ParentCardPanel.DecreaseCardsForChange();
            }
            else if (ParentCardPanel.ChangeIndex < 2)
            {
                IsForChange = true;
                transform.localScale = transform.localScale * 1.1f;
                ParentCardPanel.IncreaseCardsForChange();
            }
        }
        else
        {
            if (IsPicked)
            {
                IsPicked = false;
                transform.localScale = originScale;
            }
            else
            {
                IsPicked = true;
                transform.localScale = transform.localScale * 1.1f;
            }
        }
    }

}
