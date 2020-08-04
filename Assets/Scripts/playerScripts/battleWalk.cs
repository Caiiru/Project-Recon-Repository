using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Tilemaps;

public class battleWalk : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject playerGO; //GameObject do Player
    public Tilemap map; // tilemap
    public Transform feetPos;
    public battleSystem battleSys;
    public int LimiteDeMovimentos;
    public int LimiteDeAtaque;
    public GameObject Commandos;
    
    private bool move, canMove, movedX, diffAdded;

    public float moveSpeed;

    private Vector3 CellCenterPos;
    private bool move;



    private string playerAction;
    
    void Update()
    {
        if(canMove)
        {
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Inputar posição do mouse no mundo

            if (Input.GetButtonDown("Fire1"))
            {                                 
                Vector3Int tileCoord = map.WorldToCell(worldMousePos); //pegar input e transformar em posição do tilemap
                CellCenterPos = map.GetCellCenterWorld(tileCoord); //pegar a posição do tilemap
                feetPos.transform.position = new Vector3(CellCenterPos.x, CellCenterPos.y, 0);
                RaycastHit2D hit = Physics2D.Raycast(feetPos.transform.position, new Vector2(worldMousePos.x, worldMousePos.y));

                    if (hit.collider)
                    {
                        Debug.Log(hit.collider.gameObject.tag);

                        if (hit.collider.tag == "Player")
                        {
                            Debug.Log("CLICOU NO PLAYER");
                        }
                        else
                        {
                            if (hit.collider.tag == "Walk")
                            {
                                positionToGO = new Vector3(CellCenterPos.x, CellCenterPos.y, 0);

                                playerPosX = playerGO.transform.position.x;
                                playerPosY = playerGO.transform.position.y;

                                coordinateX = numberToNumberCount(positionToGO.x, true);
                                coordinateY = numberToNumberCount(positionToGO.y, false);
                            
                                if (limitCheck(LimiteDeMovimentos))
                                {
                                    if (playerAction == "MoveButton")
                                    {
                                        playerGO.transform.GetChild(0).gameObject.SetActive(false);
                                        move = true;
                                        changeMoveBoolToFalse();
                                    }
                                }
                            }
                            else if (hit.collider.tag == "Enemy")
                            {
                                if (limitCheck(LimiteDeAtaque))
                                {
                                    if (playerAction == "AttackButton")
                                    {
                                        Debug.Log("Enemy");
                                        changeMoveBoolToFalse();
                                        playerGO.transform.GetChild(1).gameObject.SetActive(false);
                                        battleSys.OnAttackButton();
                                    }
                                }
                            }
                            else
                            {
                                Debug.Log("aa");
                            }
                        }
                    }
                }
            if (Input.GetButtonDown("Fire2"))
            {
                if (Commandos.active == false)
                {
                    changeMoveBoolToFalse();
                    playerGO.transform.GetChild(0).gameObject.SetActive(false);
                    playerGO.transform.GetChild(1).gameObject.SetActive(false);
                    Commandos.SetActive(true);
                }
            }
        }      
        
        moveChar();
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

    private bool limitCheck(int value)
    {
        var canGo = false;
        
        var x2 = coordinateX;
        var y2 = coordinateY;
        
        if (x2 < 0)
        {
            x2 = x2 * -1;
        }
        if (y2 < 0)
        {
            y2 = y2 * -1;
        }

        if (y2 <= value && x2 <= value)
        {
            canGo = true;
        }

        return canGo;
    }
    
    public void setActionString(GameObject go)
    {
        playerAction = go.name;
        Debug.Log(playerAction);
    }
}