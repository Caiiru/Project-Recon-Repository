<<<<<<< HEAD
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Unit : MonoBehaviour
{
    // Start is called before the first frame update


    public string unitName;
    public int maxHP;
    public int currentHP;
    public int damage;
    public int charSpeed;
    public GameObject floatintextPrefab;


    public bool TakeDamage(int dmg)
    {
        currentHP -= dmg;
        if (floatintextPrefab)
        {
            showFloatingText(dmg);
        }
        if (currentHP <= 0)
            return true;
        else
            return false;

    }

    public void cureHP(int cure)
    {
        currentHP += cure;

        if(currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }
    void showFloatingText(int damage )
    {
        var go = Instantiate(floatintextPrefab, transform.position, Quaternion.identity, transform);
        go.GetComponent<TextMeshPro>().text = damage.ToString();
    }
}
=======
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // Start is called before the first frame update


    public string unitName;
    public int maxHP;
    public int currentHP;
    public int damage;

    public bool TakeDamage(int dmg)
    {
        currentHP -= dmg;

        if (currentHP <= 0)
            return true;
        else
            return false;

    }

}
>>>>>>> nathan
