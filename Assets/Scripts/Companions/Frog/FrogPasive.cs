using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogPasive : Skill
{

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpreadPoison()
    {
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(1.5f,1f));
    }
}
