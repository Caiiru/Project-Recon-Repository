using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Tilemaps;

public class battleWalk : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject playerGO; //GameObject do Player
    public Tilemap map; // tilemap
    public Transform feetPos;
    public battleSystem battleSys;
    public bool CanMove = true;
    public Grid grid;

    private Vector3Int _targetCell;
    private Vector3 _targetPosition;

    public float moveSpeed;

    private Vector3 CellCenterPos;
    private bool move;




    void Start()
    {
        Debug.Log("Game Start");

        _targetCell = grid.WorldToCell(playerGO.transform.position); //move with keyboard
        playerGO.transform.position = grid.CellToWorld(_targetCell);//move with keyboard

    }

    // Update is called once per frame
    void Update()
    {

        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Inputar posição do mouse no mundo
        

        Vector3Int gridMovement = new Vector3Int();//move with keyboard


        if (Input.GetButtonDown("Fire1"))
        {
            Vector3Int tileCoord = map.WorldToCell(worldMousePos); //pegar input e transformar em posição do tilemap
            CellCenterPos = map.GetCellCenterWorld(tileCoord); //pegar a posição do centro do tile

            feetPos.transform.position = new Vector3(CellCenterPos.x, CellCenterPos.y, 0);
            RaycastHit2D hit = Physics2D.Raycast(feetPos.transform.position, new Vector2(worldMousePos.x, worldMousePos.y));

            {
                if (hit.collider.tag == "Walk")
                {
                    move = true;
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
        
        if(Input.GetKeyDown(KeyCode.A)) //A
        {

            gridMovement.y += 1;
        }
        if (Input.GetKeyDown(KeyCode.D))//D
        {
            gridMovement.y -= 1;
        }
        if (Input.GetKeyDown(KeyCode.S))//S
        {
            gridMovement.x -= 1;
        }
        if (Input.GetKeyDown(KeyCode.W))//W
        {
            gridMovement.x += 1;
        }

        if(gridMovement != Vector3Int.zero)
        {
            _targetCell += gridMovement;
            _targetPosition = grid.GetCellCenterWorld(_targetCell);
        }

        //MoveToward(_targetPosition);

        //move with keyboard

        //moveCharacter(move);
        moveChar(move);
       
    }

    void MoveToward(Vector3 target) //move with keyboard
    {
        playerGO.transform.position = Vector3.MoveTowards(transform.position, target, 5*Time.deltaTime );
    }

    private void moveCharacter(bool truornot)
    {
        if (truornot == true)
        {

            var positionToGo = new Vector3(CellCenterPos.x,CellCenterPos.y,0);
            if (positionToGo != playerGO.transform.position)
            {
                playerGO.transform.position=Vector3.MoveTowards(playerGO.transform.position, positionToGo
                    ,1f*Time.deltaTime);
                
            }
            else
            {
                truornot=false;
            }
        }
        else
        {

        }
    }
    private void moveChar(bool boo)
    {
        if (boo)
        {
            var goToPosition = new Vector3(CellCenterPos.x,CellCenterPos.y,0);
            if (goToPosition.x != playerGO.transform.position.x )
            {
                playerGO.transform.position = Vector3.MoveTowards(playerGO.transform.position, new Vector3(goToPosition.x, goToPosition.y), 1f * Time.deltaTime);
            }
            else
            {
                
            }
        }
    }



}
