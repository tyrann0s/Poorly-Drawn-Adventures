using System;
using Cards;
using Managers;
using Mobs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Record : MonoBehaviour
{
    [SerializeField] protected Image icon;
    [SerializeField] protected TextMeshProUGUI nameText, descriptionText;
    
    public void Initialize(ElementCombo combo)
    {
        nameText.text = combo.name;
        icon.sprite = ResourceManager.Instance.Icons.GetIcon(combo.damageType);
        descriptionText.text = "";

        string condition = "";
        if (combo.elementConditions.Count > 0)
        {
            condition = $"{combo.elementConditions[0].elementType} x{combo.elementConditions.Count}";
        } else if (combo.rankConditions.Count > 0)
        {
            condition = $"x{combo.rankConditions[0].amount} of same rank";
        } else if (combo.mixedConditions.Count > 0)
        {
            condition = $"x{combo.mixedConditions[0].amount} of same rank and element";
        } else if (combo.specificConditions.Count > 0)
        {
            foreach (var cond in combo.specificConditions)
            {
                condition += $"{cond.elementType}/{cond.rank}, ";
            }
            condition = condition.Remove(condition.Length - 2);
        }
        descriptionText.text += "Condition: \n\t" + condition + "\n";
        descriptionText.text += "Damage Multiplier: x" + combo.damageMultiplier.ToString() + "\n";

        descriptionText.text += "Effects: \n";
        if (combo.stun) descriptionText.text += "\tStun for {combo.stunDuration} rounds \n";
        if (combo.ignoreDefense) descriptionText.text += "\tIgnore Defense \n";
        if (combo.aoeAttack) descriptionText.text += "\tAOE Attack \n";
    }
    
    public void Initialize(MobData mob)
    {
        nameText.text = mob.MobName;
        icon.sprite = mob.mobIcon;
        descriptionText.text = "";

        descriptionText.text += "HP: " + mob.MaxHP + "\n";
        descriptionText.text += "Stamina: " + mob.MaxStamina + "\n";
        descriptionText.text += "Attack Damage: " + mob.AttackDamage + "\n";
        switch (mob.AttackType)
        {
            case AttackType.Melee:
            case AttackType.Ranged:
                descriptionText.text += "Skill Damage: " + mob.SkillDamage + "\n";;
                break;
            case AttackType.Heal:
                descriptionText.text += "Skill: Heal " + mob.SkillDamage + "hp\n";;
                break;
            case AttackType.UnStun:
                descriptionText.text += "Skill: UnStun\n";
                break;
            case AttackType.CastShield:
                descriptionText.text += "Skill: Cast Shield\n";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        descriptionText.text += "Vulnerable To: " + mob.VulnerableTo + "\n";
        descriptionText.text += "Immune To: " + mob.ImmuneTo + "\n";
    }
}
