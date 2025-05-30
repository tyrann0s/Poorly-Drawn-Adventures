using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cards
{
    public class CardCombination : MonoBehaviour
    {
        [SerializeField]
        private List<ElementCombo> specialCombinations = new ();
        [SerializeField]
        private List<ElementCombo> mixedCombinations = new ();
        [SerializeField]
        private List<ElementCombo> elementCombinations = new ();
        [SerializeField]
        private List<ElementCombo> rankCombinations = new ();

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
            if (selectedRanks.Count == 0) return null;
        
            // Сортируем элементы и ранги для правильного сравнения
            selectedElements.Sort();
            selectedRanks.Sort();

            // Проверяем специальные комбо
            foreach (var combo in specialCombinations)
            {
                // Если выбрано больше карт чем нужно для комбинации - сразу пропускаем
                if (selectedCards.Count(c => c != null && c.IsPicked) != combo.specificConditions.Count)
                {
                    continue;
                }

                var matchedCards = new HashSet<Card>();
                bool allMatched = true;
                    
                foreach (var condition in combo.specificConditions)
                {
                    bool conditionMet = false;
                        
                    foreach (var card in selectedCards)
                    {
                        if (card != null && 
                            card.IsPicked && 
                            card.GetElement() == condition.elementType && 
                            card.GetRank() == condition.rank &&
                            !matchedCards.Contains(card))
                        {
                            matchedCards.Add(card);
                            conditionMet = true;
                            break;
                        }
                    }
                        
                    if (!conditionMet)
                    {
                        allMatched = false;
                        break;
                    }
                }
                    
                if (allMatched)
                {
                    return combo;
                }
            }
            
            // Проверяем смешанные комбо
            foreach (var combo in mixedCombinations)
            {
                foreach (var condition in combo.mixedConditions)
                {
                    // Получаем выбранные карты
                    var pickedCards = selectedCards.Where(c => c != null && c.IsPicked).ToList();
                        
                    // Проверяем что всего 4 карты при amount = 2
                    if (pickedCards.Count != condition.amount * 2)
                    {
                        continue;
                    }

                    // Перебираем каждый элемент как возможную первую группу
                    foreach (var element in pickedCards.Select(c => c.GetElement()).Distinct())
                    {
                        // Находим карты этого элемента
                        var sameElementCards = pickedCards.Where(c => c.GetElement() == element).ToList();
                            
                        // Если карт этого элемента ровно 2
                        if (sameElementCards.Count == condition.amount)
                        {
                            // Находим оставшиеся карты (другого элемента)
                            var otherCards = pickedCards.Where(c => c.GetElement() != element).ToList();
                                
                            // Группируем их по рангу
                            var rankGroups = otherCards.GroupBy(c => c.GetRank());
                                
                            // Если есть группа из 2 карт одного ранга
                            if (rankGroups.Any(g => g.Count() == condition.amount))
                            {
                                return combo;
                            }
                        }
                    }
                }
            }
            
            // Проверяем элементальные комбо
            foreach (var combo in elementCombinations)
            {
                // Проверяем количество выбранных карт
                var pickedCards = selectedCards.Where(c => c != null && c.IsPicked).ToList();
                if (pickedCards.Count != combo.elementConditions.Count) continue;
                
                // Получаем элементы выбранных карт
                var cardElements = pickedCards.Select(c => c.GetElement()).ToList();
                cardElements.Sort();
                
                // Получаем требуемые элементы из комбинации
                var requiredElements = combo.elementConditions.Select(c => c.elementType).ToList();
                requiredElements.Sort();
                
                // Сравниваем списки элементов
                if (cardElements.SequenceEqual(requiredElements))
                {
                    return combo;
                }
            }
            
            // Проверяем ранговые комбо
            foreach (var combo in rankCombinations)
            {
                foreach (var condition in combo.rankConditions)
                {
                    // Проверяем количество выбранных карт
                    var pickedCards = selectedCards.Where(c => c != null && c.IsPicked).ToList();
                    if (pickedCards.Count != condition.amount) continue;
                        
                    // Группируем карты по рангу
                    var rankGroups = pickedCards.GroupBy(c => c.GetRank());
                        
                    // Если все карты одного ранга
                    if (rankGroups.Count() == 1 && rankGroups.First().Count() == condition.amount)
                    {
                        return combo;
                    }
                }
            }

            return null;
        }
    }
}