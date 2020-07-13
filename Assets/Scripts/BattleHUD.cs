using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public void setHP(int hp)
    {
        HpSlider.value = hp;
        
    }

}
