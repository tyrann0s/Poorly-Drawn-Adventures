using Mobs;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MobButton : MonoBehaviour
{
    [SerializeField] private string description;
    
    private void OnMouseEnter()
    {

    }

    private void OnMouseExit()
    {

    }

    private void OnMouseDown()
    {
        
    }

    public void SetActive(bool value, Mob targetMob)
    {
        /*if (value)
        {
            text.color = Color.black;
        }
        else text.color = Color.gray;

        isActive = value;
        mob = targetMob;*/
    }

    public void ChangeText(string value)
    {
        //text.text = value;
    }
}
