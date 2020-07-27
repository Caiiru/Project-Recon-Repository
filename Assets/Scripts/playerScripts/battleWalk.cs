using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Net;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine.SceneManagement;

public class battleWalk : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject playerGO; //GameObject do Player
    public Tilemap map; // tilemap
    public Transform feetPos;
    public battleSystem battleSys;

    private bool move, canMove, movedX, diffAdded;

    private Vector3 CellCenterPos, positionToGO;

    private float playerPosX, playerPosY;

    private float coordinateX, coordinateY, floatX, floatY;

    private Vector3 newPos;

    private Vector3[] allPosX = new Vector3[30];
    
    private Vector3[] allPosY = new Vector3[30];
    
    void Start()
    {/*
        guerreiros.Add(GameObject.FindGameObjectWithTag("Player"));
        guerreiros.Add(GameObject.FindGameObjectWithTag("Companion1"));
        guerreiros.Add(GameObject.FindGameObjectWithTag("Companion2"));
        guerreiros.Add(GameObject.FindGameObjectWithTag("Boss"));

        guerreiros = guerreiros.OrderBy(e => e.GetComponent<Unit>().speed).ToList();

        guerreiros.Reverse();
        
        for (int x = 0; x < guerreiros.Count; x++)
        {
            Debug.Log("NOME: " + guerreiros[x].name + " VELOCIDADE: " + guerreiros[x].GetComponent<Unit>().speed);
        }*/

        Debug.Log("Game Start");
        canMove = false;
    }

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

                    if (hit.collider.tag == playerGO.tag)
                    {
                        Debug.Log("CLICOU NO PLAYER");
                    }
                    else
                    {
                        if (hit.collider.tag == "Walk")
                        {
                            move = true;
                            positionToGO = new Vector3(CellCenterPos.x, CellCenterPos.y, 0);

                            playerPosX = playerGO.transform.position.x;
                            playerPosY = playerGO.transform.position.y;

                            coordinateX = numberToNumberCount(positionToGO.x, true);
                            coordinateY = numberToNumberCount(positionToGO.y, false);
                            
                            changeMoveBoolToFalse();
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
                            ResetMoveVars();
                            changeMoveBoolToTrue();
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
           positionToGO = new Vector3(CellCenterPos.x, CellCenterPos.y, 0);
           
            if (positionToGO != playerGO.transform.position)
            {
                Debug.Log("COORX: "+coordinateX);
                Debug.Log("COORY: "+coordinateY);
                goToTile();
            }
            else
            {
                changeMoveBoolToTrue();
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
          //              Debug.Log("PASSADAS: "+passadas+"// NUMBERTOGO: "+numberToGoTo);
        //                Debug.Log("CONT: "+ cont);
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
    //                    Debug.Log("PASSADAS: "+passadas+"// NUMBERTOGO: "+numberToGoTo);
      //                  Debug.Log("CONT: "+ cont);
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
  //                      Debug.Log("PASSADAS: "+passadas+"// NUMBERTOGO: "+numberToGoTo);
//                        Debug.Log("CONT: "+ cont);
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
              //          Debug.Log("PASSADAS: "+passadas+"// NUMBERTOGO: "+numberToGoTo);
            //            Debug.Log("CONT: "+ cont);
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
                    changeMoveBoolToTrue();
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
                    changeMoveBoolToTrue();
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
                changeMoveBoolToTrue();
            }
        }
    }
}
