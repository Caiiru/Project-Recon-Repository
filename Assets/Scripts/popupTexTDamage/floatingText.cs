using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class floatingText : MonoBehaviour
{
   public float DestroyTime = 3f;

   void Start()
   {
   		Destroy(gameObject, DestroyTime);
   }
}
