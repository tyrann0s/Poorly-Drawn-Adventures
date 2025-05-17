using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentMobPanel : MonoBehaviour
{
    [SerializeField]
    private Text nameValue, hpValue, staminaValue, defenseCostValue, attack1DamageValue, attack1CostValue, attack2DamageValue, attack2CostValue;

    public void UpdatePanel(Mob mob)
    {
        nameValue.text = mob.MobData.MobName;

        hpValue.text = mob.MobHP.ToString() + "/" + mob.MobData.MaxHP;
        staminaValue.text = mob.MobStamina.ToString() + "/" + mob.MobData.MaxStamina;

        defenseCostValue.text = mob.MobData.DefenseCost.ToString() + " STM";

        attack1DamageValue.text = mob.MobData.Attack1Damage.ToString() + " DMG";
        attack1CostValue.text = mob.MobData.Attack1Cost.ToString() + " STM";

        attack2DamageValue.text = mob.MobData.Attack2Damage.ToString() + " DMG";
        attack2CostValue.text = mob.MobData.Attack2Cost.ToString() + " STM";
    }

    public void UpdatePanel()
    {
        nameValue.text = "";

        hpValue.text = "";
        staminaValue.text = "";

        defenseCostValue.text = "";

        attack1DamageValue.text = "";
        attack1CostValue.text = "";

        attack2DamageValue.text = "";
        attack2CostValue.text = "";
    }
}
