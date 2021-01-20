using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParts : MonoBehaviour
{
    private Unit[] partesDoInimigo;
    
    void Start()
    {
        partesDoInimigo = gameObject.GetComponentsInChildren<Unit>();
        
        partesDoInimigo[0] = partesDoInimigo[1];
        partesDoInimigo[1] = partesDoInimigo[2];
        partesDoInimigo[2] = partesDoInimigo[3];
        partesDoInimigo[3] = partesDoInimigo[4];
        partesDoInimigo[4] = partesDoInimigo[5];
        //partesDoInimigo[5] = partesDoInimigo[6];
        
        for (int i = 0; i < partesDoInimigo.Length; i++)
        {
            Debug.Log(partesDoInimigo[i]);
        }
    }
    
    void Update()
    {
        for (int x = 0; x < partesDoInimigo.Length; x++)
        {
            if (partesDoInimigo[x].GetComponent<Unit>().currentHP <= 0)
            {
                partesDoInimigo[x].GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 1);
            }
        }
    }

    public Unit[] ReturnAllEnemyParts()
    {
        return partesDoInimigo;
    }
}
