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
    public bool IsPicked { get; private set; } = false;

    private void Awake()
    {
        cardElement.CurrentElementType = (ElementType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(ElementType)).Length);
        cardRank = UnityEngine.Random.Range(1, 7);
    }

    private void Start()
    {
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
        if (!isHovering)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + .25f, transform.position.z);
            isHovering = true;
        }
    }


    private void OnMouseExit()
    {
        if (!IsPicked)
        {
            isHovering = false;
            transform.position = new Vector3(transform.position.x, transform.position.y - .25f, transform.position.z);
        }
    }

    private void OnMouseDown()
    {
        if (!IsPicked)
        {
            transform.localScale = transform.localScale * 1.1f;
            IsPicked = true;
        }
    }


}
