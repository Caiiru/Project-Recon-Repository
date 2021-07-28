﻿using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    public Text showName;
    public Slider HpSlider;

    public void setHUD(Unit unit)
    {
        showName.text = unit.unitName;
        HpSlider.maxValue = unit.maxHP;
        HpSlider.value = unit.currentHP;
    }

    public void setHP(float hp)
    {
        HpSlider.value = hp;       
    }
}
