using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Fly : Skill
{
    private bool usingSkill;

    public Tilemap map;
    // Start is called before the first frame update
    void Start()
    {
        usingSkill = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (usingSkill)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D raycast = Physics2D.Raycast(worldMousePosition, Vector3.forward,
                    Mathf.Infinity);
                if (raycast.collider)
                {
                    Vector3Int tileCoord = map.WorldToCell(worldMousePosition);
                    Vector3 CellCenterPos = map.GetCellCenterWorld(tileCoord);
                    transform.position = CellCenterPos;

                    Debug.Log("Y");
                }
            }

            if (Input.GetButtonDown("Fire2"))
            {
                GetComponent<battleWalk>().setSkillCommandCanvas(true);
                
            }
        }
    }
}
