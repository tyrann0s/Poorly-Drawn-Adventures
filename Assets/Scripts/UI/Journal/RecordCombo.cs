using Cards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecordCombo : Record
{
    [SerializeField] private Image icon;
    [SerializeField] TextMeshProUGUI nameText, conditionText, damageMultiplierText, effectsText;
    public override void Initialize<T>(T data)
    {
        var combo = data as ElementCombo;
        nameText.text = combo.name;

        string conditions = "";
        if (combo.elementConditions.Count > 0)
        {
            conditions = $"{combo.elementConditions[0].elementType} x{combo.elementConditions.Count}";
        } else if (combo.rankConditions.Count > 0)
        {
            conditions = $"x{combo.rankConditions[0].amount} of same rank";
        } else if (combo.mixedConditions.Count > 0)
        {
            conditions = $"x{combo.mixedConditions[0].amount} of same rank and element";
        } else if (combo.specificConditions.Count > 0)
        {
            foreach (var cond in combo.specificConditions)
            {
                conditions += $"{cond.elementType}/{cond.rank}, ";
            }
            conditions = conditions.Remove(conditions.Length - 2);
        }
        conditionText.text = "Condition: " + conditions;
        
        damageMultiplierText.text = "Damage Multiplier: x" + combo.damageMultiplier.ToString();

        effectsText.text = "Effects: \n";
        if (combo.stun) effectsText.text += "\tStun for {combo.stunDuration} rounds \n";
        if (combo.ignoreDefense) effectsText.text += "\tIgnore Defense \n";
        if (combo.aoeAttack) effectsText.text += "\tAOE Attack \n";
    }
}
