using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaspRage : Skill
{
    public static float waspDamageRate = 1f;
    void Start()
    {

    }
    private void Awake()
    {
        skillName = "Wasp Rage";
    }

    void Update()
    {
        
    }

    public void checkHP()
    {
        var HP = this.GetComponent<Unit>().currentHP;
        var maxHP = this.GetComponent<Unit>().maxHP;

        if (HP > maxHP / 2)
        {
            waspDamageRate = 1f;
        }
        else if (HP < maxHP / 2 && HP > maxHP / 4)
            waspDamageRate = 2f;
        else
            waspDamageRate = 4f;

        Debug.Log("current modifier: "+waspDamageRate);
    }
}
