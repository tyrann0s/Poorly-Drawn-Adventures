using System;
using Managers;
using Mobs.Skills;
using TMPro;
using UI.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace Items
{
    public class ScrollItem : ItemPref
    {
        [SerializeField] private bool isHostile;
        [SerializeField] private bool isAOE;
        
        private Image iconImage;

        public override void Initialize(Item item, Sprite icon, ItemScroll scroll)
        {
            base.Initialize(item, icon, scroll);
            iconImage = GetComponent<Image>();
            iconImage.sprite = icon;

            TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();
            text.text = item.Description;

            if (!isAOE)
            {
                //ServiceLocator.Get<GameManager>().CurrentPhase = GamePhase.AssignActions;
                /*if (isHostile) ServiceLocator.Get<GameManager>().SelectingState = SelectingState.Enemy;
                else ServiceLocator.Get<GameManager>().SelectingState = SelectingState.Player;*/
            }
        }
    }
}
