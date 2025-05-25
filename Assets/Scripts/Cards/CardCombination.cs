using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cards
{
    public class CardCombination : MonoBehaviour
    {
        [SerializeField]
        private List<ElementCombo> availableCombinations = new ();

        public ElementCombo CheckCombination(List<Card> selectedCards)
        {
            // Получаем типы элементов выбранных карт
            List<ElementType> selectedElements = selectedCards
                .Where(card => card != null && card.IsPicked)
                .Select(card => card.GetElement())
                .ToList();
            
            // Получаем ранги выбранных карт
            List<int> selectedRanks = selectedCards
                .Where(card => card != null && card.IsPicked)
                .Select(card => card.GetRank())
                .ToList();
        
            // Если карты не выбраны, возвращаем null
            if (selectedElements.Count == 0) return null;
        
            // Сортируем элементы и ранги для правильного сравнения
            selectedElements.Sort();
            selectedRanks.Sort();
        
            // Проверяем каждую доступную комбинацию
            foreach (var combo in availableCombinations)
            {
                if (combo.useRanksInsteadOfElements)
                {
                    var comboRanks = new List<int>(combo.ranks);
                    comboRanks.Sort();
                    if (selectedRanks.SequenceEqual(comboRanks))
                    {
                        return combo;
                    }
                }
                else
                {
                    var comboElements = new List<ElementType>(combo.elements);
                    comboElements.Sort();
                    if (selectedElements.SequenceEqual(comboElements))
                    {
                        return combo;
                    }
                }
            }

            return null;
        }
    }
}