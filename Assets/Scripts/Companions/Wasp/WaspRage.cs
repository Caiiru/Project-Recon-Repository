using UnityEngine;

public class WaspRage : Skill
{
    public static float waspDamageRate = 1f;

    private bool effectApplied;

    private Unit unit;

    private float originalDamage;
    
    private void Awake()
    {
        skillName = "Wasp Rage";
        unit = gameObject.GetComponent<Unit>();
        originalDamage = unit.damage;
    }

    private void Update()
    {
        if (effectApplied == false)
        {
            checkHP();
            ApplyDamageBonus();
            effectApplied = true;
        }
    }

    private void ApplyDamageBonus()
    {
        unit.damage = (int)originalDamage;
        var damageToApply = unit.damage + waspDamageRate;
        unit.damage = (int)damageToApply;
    }
    
    private void checkHP()
    {
        var HP = unit.currentHP;
        
        var maxHP = unit.maxHP;

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

    public void SetEffectApplied(bool value)
    {
        effectApplied = value;
    }
}
