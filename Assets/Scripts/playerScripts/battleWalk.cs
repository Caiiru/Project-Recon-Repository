using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class battleWalk : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject playerGO; //GameObject do Player
    public Tilemap map; // tilemap
    public Transform feetPos;
    public battleSystem battleSys;
    void Start()
    {
        Debug.Log("Game Start");
        battleSys = GetComponent<battleSystem>();
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Inputar posição do mouse no mundo

        if (Input.GetButtonDown("Fire1"))
        {
            Vector3Int tileCoord = map.WorldToCell(worldMousePos); //pegar input e transformar em posição do tilemap
            Vector3 CellCenterPos = map.GetCellCenterWorld(tileCoord); //pegar a posição do tilemap
            feetPos.transform.position = new Vector3(CellCenterPos.x, CellCenterPos.y, 0);
            RaycastHit2D hit = Physics2D.Raycast(feetPos.transform.position, new Vector2(worldMousePos.x, worldMousePos.y));
            {
                if (hit.collider.tag == "Walk")
                {
                    
                    playerGO.transform.position = new Vector3(CellCenterPos.x, CellCenterPos.y, 0);
                    //playerGO.transform.position = new Vector3(feetPos.x, feetPos.y, 0);
                }

                else if (hit.collider.isTrigger)
                {
                    Debug.Log("Enemy");
                    battleSys.OnAttackButton();
                }
                
                else
                {
                    Debug.Log("aa");
                }

            }
        }
    }
}
