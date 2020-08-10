using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParts : MonoBehaviour
{
    private Unit[] partesDoInimigo;
    
    void Start()
    {
        partesDoInimigo = this.gameObject.GetComponentsInChildren<Unit>();
        
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
}
