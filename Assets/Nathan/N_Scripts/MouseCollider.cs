using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCollider : MonoBehaviour
{
    void OnMouseOver()
    {
        Debug.Log("PASSOU EM "+gameObject.name);
    }
}
