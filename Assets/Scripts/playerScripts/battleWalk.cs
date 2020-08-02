<<<<<<< HEAD
﻿using System.Collections;
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
=======
﻿using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class battleWalk : MonoBehaviour
{
    public GameObject playerGO; //GameObject do Player
    public Tilemap map; // tilemap
    public Transform feetPos;
    public battleSystem battleSys;
    public int LimiteDeMovimentos;

    private bool move, canMove, movedX, diffAdded;

    private Vector3 CellCenterPos, positionToGO, newPos;

    private float playerPosX, playerPosY;

    private float coordinateX, coordinateY, floatX, floatY;

    private Vector3[] allPosX = new Vector3[30];
    
    private Vector3[] allPosY = new Vector3[30];

    private GameObject newTile;

    
    void Update()
    
    {
        if (Input.GetButtonDown("Fire2"))
        {
            SceneManager.LoadScene(0);
        }
        
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

                            if (limitCheck())
                            {
                                playerGO.transform.GetChild(0).gameObject.SetActive(false);
                                move = true;
                                changeMoveBoolToFalse();
                            }
                        }
                        else if (hit.collider.isTrigger)
                        {
                            Debug.Log("Enemy");
                            changeMoveBoolToFalse();
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
        
        moveChar();
    }

    private void moveChar()
    {
        if (move)
        {
           if (positionToGO != playerGO.transform.position)
           {
               Debug.Log("COORX: "+coordinateX);
               Debug.Log("COORY: "+coordinateY);
               goToTile();
           }
           else
           {
               ResetMoveVars();
           }
        }
    }
    
    public void changeMoveBoolToTrue()
    {
        canMove = true;
    }
    
    public void changeMoveBoolToFalse()
    {
        canMove = false;
    }

    public int numberToNumberCount(float numberToGoTo, bool isX)
    {
        var passadas = playerPosX;
        
        var cont = 0;
        
        if (isX)
        {
            if (passadas < numberToGoTo)
            {
                passadas = playerPosX;
                var infiniteCount = 200;
                for (int i = 0; i < infiniteCount; i++)
                {
                    if (passadas != numberToGoTo)
                    {
                        passadas = passadas + 0.5f;
                        cont++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                passadas = playerPosX;
                var infiniteCount = 200;
                for (int i = 0; i < infiniteCount; i++)
                {
                    if (passadas != numberToGoTo)
                    {
                        passadas = passadas - 0.5f;
                        cont--;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        else
        {
            passadas = playerPosY;
            
            if (passadas < numberToGoTo)
            {
                passadas = playerPosY;
                var infiniteCount = 200;
                for (int i = 0; i < infiniteCount; i++)
                {
                    if (passadas != numberToGoTo)
                    {
                        passadas = passadas + 0.25f;
                        cont++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                passadas = playerPosY;
                var infiniteCount = 200;
                for (int i = 0; i < infiniteCount; i++)
                {
                    if (passadas != numberToGoTo)
                    {
                        passadas = passadas - 0.25f;
                        cont--;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        
        return cont;
    }

    private void goToTile()
    {
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
        
        if (x2 == y2)
        {
            floatX = goToTileFOR2(coordinateX, true);
            floatY = goToTileFOR2(coordinateY, false);
                            
            newPos = new Vector3(playerPosX + floatX, playerPosY + floatY, 0);
                
            playerGO.transform.position =
                Vector3.MoveTowards(playerGO.transform.position, newPos, Time.deltaTime * 5);
                
            if (playerGO.transform.position == newPos)
            {
                ResetMoveVars();
            }
        }
        else if (coordinateX > 0 && coordinateY == 0 || coordinateX < 0 && coordinateY == 0)
        {
            if (movedX == false)
            {
                if (diffAdded == false)
                {
                    floatX = goToTileFOR(coordinateX, true);
                    floatY = goToTileFOR(coordinateY, false);

                    var halfX = coordinateX / 2;
                    
                    if (halfX < 0)
                    {
                        halfX = halfX * -1;  
                        Debug.Log("HALFX: " + halfX);
                    }
                    
                    for (int z = 0; z < halfX; z++)
                    {
                        floatY = floatY + 0.25f;
                        floatX = floatX - 0.5f;
                        Debug.Log("FLOATX: " + floatX);
                        Debug.Log("FLOATY: " + floatY);
                    }
                    diffAdded = true;
                }

                newPos = new Vector3(playerPosX + floatX, playerPosY + floatY, 0);

                playerGO.transform.position =
                    Vector3.MoveTowards(playerGO.transform.position, newPos, Time.deltaTime * 5);

                if (playerGO.transform.position == newPos)
                {
                    movedX = true;
                    Debug.Log(movedX);
                }
            }
            else
            { 
                newPos = new Vector3(positionToGO.x, positionToGO.y, 0);
                
                playerGO.transform.position =
                    Vector3.MoveTowards(playerGO.transform.position, newPos, Time.deltaTime * 5);
                
                if (playerGO.transform.position == newPos)
                {
                    ResetMoveVars();
                }
            }
        }
        else if (coordinateY > 0 && coordinateX == 0 || coordinateY < 0 && coordinateX == 0)
        {
            if (movedX == false)
            {
                if (diffAdded == false)
                {
                    floatX = goToTileFOR(coordinateX, true);
                    floatY = goToTileFOR(coordinateY, false);

                    var halfY = coordinateY / 2;
                    
                    if (halfY < 0)
                    {
                        halfY = halfY * -1;  
                        Debug.Log("HALFY: " + halfY);
                    }
                    
                    for (int z = 0; z < halfY; z++)
                    {
                        floatY = floatY - 0.25f;
                        floatX = floatX - 0.5f;
                        Debug.Log("FLOATX: " + floatX);
                        Debug.Log("FLOATY: " + floatY);
                    }
                    diffAdded = true;
                }

                newPos = new Vector3(playerPosX + floatX, playerPosY + floatY, 0);

                playerGO.transform.position =
                    Vector3.MoveTowards(playerGO.transform.position, newPos, Time.deltaTime * 5);

                if (playerGO.transform.position == newPos)
                {
                    movedX = true;
                    Debug.Log(movedX);
                }
            }
            else
            { 
                newPos = new Vector3(positionToGO.x, positionToGO.y, 0);
                
                playerGO.transform.position =
                    Vector3.MoveTowards(playerGO.transform.position, newPos, Time.deltaTime * 5);
                
                if (playerGO.transform.position == newPos)
                {
                    ResetMoveVars();
                }
            }
        }
        else if(coordinateX > 0 && coordinateY > 0)
        {
            ///////GRAPH X+ Y+
            ///////GRAPH X+ Y-
            if (coordinateX > coordinateY)
            {
                calculateTileAdjustment(4);
            }
            else
            {
                calculateTileAdjustment(5);
            }
        }
        else if(coordinateX > 0 && coordinateY < 0)
        {
            var yy = coordinateY;
            
            if (coordinateY < 0)
            {
                yy = coordinateY * -1;
            }
            
            if (yy > coordinateX)
            {
                calculateTileAdjustment(6);
            }
            else
            {
                calculateTileAdjustment(7);
            }
        }
        else if(coordinateX < 0 && coordinateY < 0)
        {
            ///////GRAPH X- Y-
            if (coordinateX < coordinateY)
            {
                calculateTileAdjustment(3);
            }
            else
            {
                calculateTileAdjustment(2);
            }
        }
        else if(coordinateY > 0 && coordinateX < 0)
        {
            ///////GRAPH X- Y+
            var xx = coordinateX * -1;
            if (xx > coordinateY)
            {
                calculateTileAdjustment(1);
            }
            else
            {
                calculateTileAdjustment(0);
            }
        }
    }

    private float goToTileFOR(float indexToRun, bool isX)
    {
        var floatzada = 0f;

        var soma = 0f;
        
        if (isX)
        {
            soma = 0.5f;
        }
        else
        {
            soma = 0.25f;
        }

        if (indexToRun >= 0)
        {
            for (int i = 0; i < indexToRun; i++)
            {
                floatzada = floatzada + soma;
            }
        }
        
        return floatzada;
    }
    
    private float goToTileFOR2(float indexToRun, bool isX)
    {
        var floatzada = 0f;

        var soma = 0f;
        
        if (isX)
        {
            soma = 0.5f;
        }
        else
        {
            soma = 0.25f;
        }

        if (indexToRun >= 0)
        {
            for (int i = 0; i < indexToRun; i++)
            {
                floatzada = floatzada + soma;
            }
        }
        else
        {
            for (int i = 0; i > indexToRun; i--)
            {
                floatzada = floatzada - soma;
            }
        }
        
        return floatzada;
    }

    private void ResetMoveVars()
    {
        move = false;
        movedX = false;
        diffAdded = false;
        changeMoveBoolToFalse();
        battleSys.activateCommandsMenu();
    }

    private void getAllTilePositions(int direction)
    {
        float posXclone = playerGO.transform.position.x;
        float posYclone = playerGO.transform.position.y;

        float posXclone2 = posXclone + goToTileFOR2(coordinateX, true);
        float posYclone2 = posYclone + goToTileFOR2(coordinateY, false);
        
        Debug.Log("POSXCLONE: "+ posXclone2);
        Debug.Log("POSYCLONE: "+ posYclone2);
        
        var somaX = 0.5f;
        var somaY = 0.25f;

        for (int z = 0; z < 30; z++)
        {
            switch (direction)
            {
                case 0:
                    allPosX[z] = new Vector3(posXclone + somaX, posYclone + somaY, 0);
                    allPosY[z] = new Vector3(posXclone2 + somaX, posYclone2 - somaY, 0);
                    posXclone = posXclone + somaX;
                    posYclone = posYclone + somaY;
                    posXclone2 = posXclone2 + somaX; 
                    posYclone2 = posYclone2 - somaY;
                break;
                case 1:
                    allPosX[z] = new Vector3(posXclone - somaX, posYclone - somaY, 0);
                    allPosY[z] = new Vector3(posXclone2 + somaX, posYclone2 - somaY, 0);
                    posXclone = posXclone - somaX;
                    posYclone = posYclone - somaY;
                    posXclone2 = posXclone2 + somaX; 
                    posYclone2 = posYclone2 - somaY;
                break;
                case 2:
                    allPosX[z] = new Vector3(posXclone - somaX, posYclone - somaY, 0);
                    allPosY[z] = new Vector3(posXclone2 - somaX, posYclone2 + somaY, 0);
                    posXclone = posXclone - somaX;
                    posYclone = posYclone - somaY;
                    posXclone2 = posXclone2 - somaX; 
                    posYclone2 = posYclone2 + somaY;
                break;
                case 3:
                    allPosX[z] = new Vector3(posXclone - somaX, posYclone - somaY, 0);
                    allPosY[z] = new Vector3(posXclone2 + somaX, posYclone2 - somaY, 0);
                    posXclone = posXclone - somaX;
                    posYclone = posYclone - somaY;
                    posXclone2 = posXclone2 + somaX; 
                    posYclone2 = posYclone2 - somaY;
                break;
                case 4:
                    allPosX[z] = new Vector3(posXclone + somaX, posYclone + somaY, 0);
                    allPosY[z] = new Vector3(posXclone2 - somaX, posYclone2 + somaY, 0);
                    posXclone = posXclone + somaX;
                    posYclone = posYclone + somaY;
                    posXclone2 = posXclone2 - somaX; 
                    posYclone2 = posYclone2 + somaY;
                break;
                case 5:
                    allPosX[z] = new Vector3(posXclone + somaX, posYclone + somaY, 0);
                    allPosY[z] = new Vector3(posXclone2 + somaX, posYclone2 - somaY, 0);
                    posXclone = posXclone + somaX;
                    posYclone = posYclone + somaY;
                    posXclone2 = posXclone2 + somaX; 
                    posYclone2 = posYclone2 - somaY;
                break;
                case 6:
                    allPosX[z] = new Vector3(posXclone + somaX, posYclone - somaY, 0);
                    allPosY[z] = new Vector3(posXclone2 + somaX, posYclone2 + somaY, 0);
                    posXclone = posXclone + somaX;
                    posYclone = posYclone - somaY;
                    posXclone2 = posXclone2 + somaX; 
                    posYclone2 = posYclone2 + somaY;
                break;
                case 7:
                    allPosX[z] = new Vector3(posXclone + somaX, posYclone + somaY, 0);
                    allPosY[z] = new Vector3(posXclone2 - somaX, posYclone2 + somaY, 0);
                    posXclone = posXclone + somaX;
                    posYclone = posYclone + somaY;
                    posXclone2 = posXclone2 - somaX; 
                    posYclone2 = posYclone2 + somaY;
                break;
            }
        }
    }

    private void checkAllTilePositions()
    {
        for (int z = 0; z < allPosX.Length; z++)
        {
            for (int c = 0; c < allPosY.Length; c++)
            {
                if (allPosX[z] == allPosY[c])
                {
                    newPos = allPosX[z];
                }
            }
        }
    }    
    
    private void calculateTileAdjustment(int direction)
    {
        if (movedX == false)
        {
            if (diffAdded == false)
            {
                getAllTilePositions(direction);
                checkAllTilePositions();
                diffAdded = true;
            }

            playerGO.transform.position =
                Vector3.MoveTowards(playerGO.transform.position, newPos, Time.deltaTime * 5);

            if (playerGO.transform.position == newPos)
            {
                movedX = true;
                Debug.Log("IGUAL");
            }
        }
        else
        {
            playerGO.transform.position =
                Vector3.MoveTowards(playerGO.transform.position, positionToGO, Time.deltaTime * 5);
            if (playerGO.transform.position == positionToGO)
            {
                ResetMoveVars();
            }
        }
    }

    private bool limitCheck()
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

        if (y2 <= LimiteDeMovimentos && x2 <= LimiteDeMovimentos)
        {
            canGo = true;
        }

        return canGo;
    }
}
>>>>>>> nathan
