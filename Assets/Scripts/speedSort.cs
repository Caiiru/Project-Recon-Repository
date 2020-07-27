using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class speedSort : MonoBehaviour
{


    public List<GameObject> personagens = new List<GameObject>(4);

    void Start()
    {
        

    }


    public void SortList()
    {
        personagens.Add(GameObject.FindGameObjectWithTag("Player"));
        personagens.Add(GameObject.FindGameObjectWithTag("Companion1"));
        personagens.Add(GameObject.FindGameObjectWithTag("Companion2"));
        personagens.Add(GameObject.FindGameObjectWithTag("Enemy"));

        personagens = personagens.OrderBy(e => e.GetComponent<Unit>().charSpeed).ToList();

        personagens.Reverse();



        for (int i=0; i<personagens.Count; i++)
        {
            Debug.Log(personagens[i].GetComponent<Unit>().unitName + " velocidade: "+personagens[i].GetComponent<Unit>().charSpeed);
        }
    }

    public void DeleteList()
    {
        personagens.Clear();
    }
}
