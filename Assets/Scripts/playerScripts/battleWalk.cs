using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using Button = UnityEngine.UI.Button;

public class battleWalk : MonoBehaviour
{
    public GameObject playerGO; //GameObject do Player
    public Tilemap map; // tilemap
    public Transform feetPos;
    public battleSystem battleSys;
    public int limiteDeMovimento;
    public int limiteDeAtaque;
    public GameObject Commandos;
    public LayerMask layerMask;
    
    private string playerAction;
    
    private bool move, canMove, movedX, diffAdded;

    private float playerPosX, playerPosY, coordinateX, coordinateY, floatX, floatY;
    
    private Vector3 CellCenterPos, positionToGO, newPos;

    private Vector3[] allPosX = new Vector3[30], allPosY = new Vector3[30];

    private Button _moveButton;

    private void Start()
    {
        var go = Commandos.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        _moveButton = go.GetComponent<Button>();
    }

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
                
                RaycastHit2D hit = Physics2D.Raycast(feetPos.transform.position, new Vector2(worldMousePos.x, worldMousePos.y), Mathf.Infinity, layerMask);     
                
                if (hit.collider)
                {
                    Debug.Log(hit.collider.gameObject.tag);

                    if (hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("EnemyPart"))
                    {
                        if (playerAction == "MoveButton")
                        {
                            gameObject.GetComponent<Unit>().playSound(4);
                        }
                        
                        positionToGO = new Vector3(CellCenterPos.x, CellCenterPos.y, playerGO.transform.position.z);
                        
                        playerPosX = playerGO.transform.position.x;
                        playerPosY = playerGO.transform.position.y;
                                                                            
                        coordinateX = NumberToNumberCount(positionToGO.x, true);
                        coordinateY = NumberToNumberCount(positionToGO.y, false);
                            
                        if (LimitCheck(limiteDeAtaque))
                        {
                            if (playerAction == "AttackButton")
                            {
                                Debug.Log("Enemy foi atacado");
                                ChangeMoveBool(false);
                                Debug.Log(hit.collider.gameObject);
                                playerGO.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(false);
                                battleSys.OnAttackButton(hit.collider.gameObject);
                                _moveButton.interactable = true;
                            }
                        }
                        else
                        {
                            gameObject.GetComponent<Unit>().playSound(4);
                        }
                    }
                    else if (hit.collider.CompareTag("Walk"))
                    {   
                        if (playerAction == "AttackButton")
                        {
                            gameObject.GetComponent<Unit>().playSound(4);
                        }

                        positionToGO = new Vector3(CellCenterPos.x, CellCenterPos.y, playerGO.transform.position.z);
                        
                        Debug.Log(positionToGO.x);
                        Debug.Log(positionToGO.y);
                        
                        playerPosX = playerGO.transform.position.x;
                        playerPosY = playerGO.transform.position.y;

                        coordinateX = NumberToNumberCount(positionToGO.x, true);
                        coordinateY = NumberToNumberCount(positionToGO.y, false);
                        
                        if (LimitCheck(limiteDeMovimento))
                        {
                            if (playerAction == "MoveButton")
                            {
                                playerGO.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                                move = true;
                                ChangeMoveBool(false);
                                gameObject.GetComponent<Unit>().playSound(0);
                                _moveButton.interactable = false;
                            }
                        }
                        else
                        {
                            gameObject.GetComponent<Unit>().playSound(4);
                        }
                    }
                    else
                    {
                        Debug.Log("aa");
                        gameObject.GetComponent<Unit>().playSound(4);
                    }
                }
            }
            if (Input.GetButtonDown("Fire2"))
            {
                if (!Commandos.activeSelf)
                {
                    ChangeMoveBool(false);
                    var playerGo = gameObject.transform.GetChild(0);
                    playerGo.transform.GetChild(0).gameObject.SetActive(false);
                    playerGo.transform.GetChild(1).gameObject.SetActive(false);
                    Commandos.SetActive(true);
                }
            }
        }      
        
        MoveChar();
    }

    private void MoveChar()
    {
        if (move)
        {
           if (positionToGO != playerGO.transform.position)
           {
               GoToTile();
           }
           else
           {
               ResetMoveVars();
           }
        }

        if (Commandos.activeSelf)
        {
            if(gameObject.GetComponent<AudioSource>().isPlaying && gameObject.GetComponent<AudioSource>().clip.name == "walkingPlaceHolder")
            {
                gameObject.GetComponent<Unit>().stopSound();
            }
        }
    }
    
    public void ChangeMoveBool(bool toChange)
    {
        canMove = toChange;
    }
    
    private int NumberToNumberCount(float numberToGoTo, bool isX)
    {
        var passadas = playerPosX;
        
        var cont = 0;

        var toCount = 0f;

        if (isX)
        {
            toCount = 0.5f;
        }
        else
        {
            toCount = 0.25f;
            passadas = playerPosY;
        }        
        
        for (int i = 0; i < 200; i++)
        {
            if (passadas != numberToGoTo)
            {
                if (passadas < numberToGoTo)
                {
                    passadas = passadas + toCount;
                    cont++;
                }
                else
                {
                    passadas = passadas - toCount;
                    cont--;
                }
                Debug.Log("PASSADAS: " + passadas + " ///NumberToGo: " + numberToGoTo);
            }
            else
            {
                break;
            }
        }
        
        return cont;
    }

    private void GoToTile()
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
                            
            newPos = new Vector3(playerPosX + floatX, playerPosY + floatY, playerGO.transform.position.z);
                
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

                newPos = new Vector3(playerPosX + floatX, playerPosY + floatY, playerGO.transform.position.z);

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
                newPos = new Vector3(positionToGO.x, positionToGO.y, playerGO.transform.position.z);
                
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

                newPos = new Vector3(playerPosX + floatX, playerPosY + floatY, playerGO.transform.position.z);

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
                newPos = new Vector3(positionToGO.x, positionToGO.y, playerGO.transform.position.z);
                
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
                CalculateTileAdjustment(3);
            }
            else
            {
                CalculateTileAdjustment(0);
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
                CalculateTileAdjustment(4);
            }
            else
            {
                CalculateTileAdjustment(3);
            }
        }
        else if(coordinateX < 0 && coordinateY < 0)
        {
            ///////GRAPH X- Y-
            if (coordinateX < coordinateY)
            {
                CalculateTileAdjustment(1);
            }
            else
            {
                CalculateTileAdjustment(2);
            }
        }
        else if(coordinateY > 0 && coordinateX < 0)
        {
            ///////GRAPH X- Y+
            var xx = coordinateX * -1;
            if (xx > coordinateY)
            {
                CalculateTileAdjustment(1);
            }
            else
            {
                CalculateTileAdjustment(0);
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

        float soma;
        
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
        ChangeMoveBool(false);
        Commandos.SetActive(true);
    }
    
    private void GetAllTilePositions(int direction)
    {
        float posXclone = gameObject.transform.position.x;
        float posYclone = gameObject.transform.position.y;

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
                    allPosX[z] = new Vector3(posXclone + somaX, posYclone + somaY, gameObject.transform.position.z);
                    allPosY[z] = new Vector3(posXclone2 + somaX, posYclone2 - somaY, gameObject.transform.position.z);
                    posXclone = posXclone + somaX;
                    posYclone = posYclone + somaY;
                    posXclone2 = posXclone2 + somaX; 
                    posYclone2 = posYclone2 - somaY;
                break;
                case 1:
                    allPosX[z] = new Vector3(posXclone - somaX, posYclone - somaY, gameObject.transform.position.z);
                    allPosY[z] = new Vector3(posXclone2 + somaX, posYclone2 - somaY, gameObject.transform.position.z);
                    posXclone = posXclone - somaX;
                    posYclone = posYclone - somaY;
                    posXclone2 = posXclone2 + somaX; 
                    posYclone2 = posYclone2 - somaY;
                break;
                case 2:
                    allPosX[z] = new Vector3(posXclone - somaX, posYclone - somaY, gameObject.transform.position.z);
                    allPosY[z] = new Vector3(posXclone2 - somaX, posYclone2 + somaY, gameObject.transform.position.z);
                    posXclone = posXclone - somaX;
                    posYclone = posYclone - somaY;
                    posXclone2 = posXclone2 - somaX; 
                    posYclone2 = posYclone2 + somaY;
                break;
                case 3:
                    allPosX[z] = new Vector3(posXclone + somaX, posYclone + somaY, gameObject.transform.position.z);
                    allPosY[z] = new Vector3(posXclone2 - somaX, posYclone2 + somaY, gameObject.transform.position.z);
                    posXclone = posXclone + somaX;
                    posYclone = posYclone + somaY;
                    posXclone2 = posXclone2 - somaX; 
                    posYclone2 = posYclone2 + somaY;
                break;
                case 4:
                    allPosX[z] = new Vector3(posXclone + somaX, posYclone - somaY, gameObject.transform.position.z);
                    allPosY[z] = new Vector3(posXclone2 + somaX, posYclone2 + somaY, gameObject.transform.position.z);
                    posXclone = posXclone + somaX;
                    posYclone = posYclone - somaY;
                    posXclone2 = posXclone2 + somaX; 
                    posYclone2 = posYclone2 + somaY;
                break;
            }
        }
    }

    private void CheckAllTilePositions()
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
    
    private void CalculateTileAdjustment(int direction)
    {
        if (movedX == false)
        {
            if (diffAdded == false)
            {
                GetAllTilePositions(direction);
                CheckAllTilePositions();
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

    private bool LimitCheck(int value)
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
    
    public void SetActionString(GameObject go)
    {
        playerAction = go.name;
        Debug.Log(playerAction);
    }
}