using DG.Tweening;
using Mobs;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MobButton : MonoBehaviour
{
    [SerializeField]
    private float destinationX, destinationY;
    public Vector3 Destination() { return new Vector3(destinationX, destinationY, 0); }

    private MobButtons buttons;

    private bool isActive;

    private Mob mob;

    [SerializeField]
    private TMP_Text text;

    [SerializeField]
    private UnityEvent action;
    
    private UISounds uiSounds; 

    private void Start()
    {
        buttons = GetComponentInParent<MobButtons>();
        uiSounds = FindFirstObjectByType<UISounds>();
    }

    private void OnMouseEnter()
    {
        if (isActive)
        {
            transform.DOScale(buttons.ScaleAmount + .2f, .3f);
            uiSounds.ButtonHover();
        }
    }

    private void OnMouseExit()
    {
        if (isActive) transform.DOScale(buttons.ScaleAmount, .3f);
    }

    private void OnMouseDown()
    {
        if (isActive)
        {
            buttons.HideButtons();
            action.Invoke();
            uiSounds.ButtonClick();
        }
        else mob.UI.ShowText("NOT ENOUGH STAMINA", Color.white);
    }

    public void SetActive(bool value, Mob targetMob)
    {
        if (value)
        {
            text.color = Color.black;
        }
        else text.color = Color.gray;

        isActive = value;
        mob = targetMob;
    }

    public void ChangeText(string value)
    {
        text.text = value;
    }
    
    private void OnDestroy()
    {
        // Останавливает все твины, связанные с этим объектом
        DOTween.Kill(transform);
    }
}
