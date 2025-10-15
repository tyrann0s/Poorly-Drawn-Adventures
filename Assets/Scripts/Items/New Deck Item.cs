using System.Collections;
using Cards;
using DG.Tweening;
using UI.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Items
{
    public class NewDeckItem : ItemPref
    {
        public override void Initialize(Item item)
        {
            base.Initialize(item);
        }

        public override void Use()
        {
            base.Use();
            //ServiceLocator.Get<UIManager>().ExitChangeCards();
            ServiceLocator.Get<CardPanel>().ResetDeck();
            parentItem.UseComplete();
        }
    }
}