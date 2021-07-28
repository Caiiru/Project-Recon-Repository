using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class s_skill_slash : so_baseSkill
{
    // Start is called before the first frame update
    public LayerMask skillLayerMask;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (UseSkill)
        {
            Debug.Log("Inside Update");
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D raycast = Physics2D.Raycast(worldMousePos, Vector3.forward, Mathf.Infinity, skillLayerMask);

            if (raycast.collider.CompareTag("Skill"))
            {
                Debug.Log("Skill");
            }
        }

        
    }
    public override void Use()
    {
        base.Use();
        UseSkill = true;
        Debug.Log("Using This Skill");
    }
}
