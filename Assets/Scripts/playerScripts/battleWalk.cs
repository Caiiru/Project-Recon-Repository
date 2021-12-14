using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using Button = UnityEngine.UI.Button;
using UnityEngine.UI;

public class battleWalk : MonoBehaviour
{
    public GameObject playerGO; //GameObject do Player
    public Tilemap map; // tilemap
    public Transform feetPos;
    public battleSystem battleSys;
    public float limiteDeMovimento;
    public GameObject Commandos;
    public GameObject ComandosSkills;
    public LayerMask layerMask, arenaMask;
    
    private string playerAction;

    private bool move, canMove, movedX, diffAdded;

    private float playerPosX, playerPosY, coordinateX, coordinateY, floatX, floatY;
    
    private Vector3 CellCenterPos, positionToGO, newPos;

    private Vector3[] allPosX = new Vector3[30], allPosY = new Vector3[30];

    private Button _moveButton, _attackButton;
    
    private Animator anim;
    
    public bool myTurn, isBurning, activateCommandsCanvas;

    private void Start()
    {
        var go = Commandos.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;
        var go2 = Commandos.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject;
        _moveButton = go.GetComponent<Button>();
        _attackButton = go2.GetComponent<Button>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(move)
        {
            anim.SetBool("isMoving",true);
        }
        else
        {
            anim.SetBool("isMoving",false);
        }

        if (activateCommandsCanvas)
        {
            if (battleSys.CheckForAllUnitsAnimation())
            {
                Commandos.SetActive(true);
                _moveButton.interactable = true;
                _attackButton.interactable = true;
                myTurn = true;
                activateCommandsCanvas = false;
            }
        }
        else
        {
            if (canMove)
            {
                Vector3 worldMousePos =
                    Camera.main.ScreenToWorldPoint(Input.mousePosition); // Inputar posição do mouse no mundo

                if (Input.GetButtonDown("Fire1"))
                {
                    Vector3Int
                        tileCoord = map.WorldToCell(worldMousePos); //pegar input e transformar em posição do tilemap
                    CellCenterPos = map.GetCellCenterWorld(tileCoord); //pegar a posição do tilemap
                    feetPos.transform.position = new Vector3(CellCenterPos.x, CellCenterPos.y, 0);

                    if (playerAction == "AttackButton")
                    {
                        RaycastHit2D hit = Physics2D.Raycast(feetPos.transform.position,
                            new Vector2(worldMousePos.x, worldMousePos.y), Mathf.Infinity, layerMask);

                        if (hit && hit.collider)
                        {
                            if (hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("EnemyPart"))
                            {
                                Debug.Log(hit.collider.gameObject.tag);

                                if (playerAction == "MoveButton")
                                {
                                    gameObject.GetComponent<Unit>().playSound(4);
                                }

                                positionToGO = new Vector3(CellCenterPos.x, CellCenterPos.y,
                                    playerGO.transform.position.z);

                                playerPosX = playerGO.transform.position.x;
                                playerPosY = playerGO.transform.position.y;

                                coordinateX = NumberToNumberCount(positionToGO.x, true);
                                coordinateY = NumberToNumberCount(positionToGO.y, false);

                                if (LimitCheckAttack(new Vector2(positionToGO.x, positionToGO.y)) &&
                                    _attackButton.interactable)
                                {
                                    Debug.Log("Enemy foi atacado");
                                    Debug.Log(hit.collider.gameObject);
                                    playerGO.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject
                                        .SetActive(false);
                                    battleSys.OnAttackButton(hit.collider.gameObject);
                                    _attackButton.interactable = false;
                                }
                                else
                                {
                                    gameObject.GetComponent<Unit>().playSound(4);
                                }
                            }
                        }
                        else
                        {
                            Debug.Log("Hitting Nothing!");
                            gameObject.GetComponent<Unit>().playSound(4);
                        }
                    }
                    else if (playerAction == "MoveButton")
                    {
                        RaycastHit2D hit2 = Physics2D.Raycast(feetPos.transform.position,
                            new Vector2(worldMousePos.x, worldMousePos.y), Mathf.Infinity, arenaMask);

                        if (hit2 && hit2.collider && hit2.collider.CompareTag("Walk"))
                        {
                            Debug.Log(hit2.collider.gameObject.tag);

                            positionToGO = new Vector3(CellCenterPos.x, CellCenterPos.y, playerGO.transform.position.z);

                            Debug.Log(positionToGO.x);
                            Debug.Log(positionToGO.y);

                            playerPosX = playerGO.transform.position.x;
                            playerPosY = playerGO.transform.position.y;

                            coordinateX = NumberToNumberCount(positionToGO.x, true);
                            coordinateY = NumberToNumberCount(positionToGO.y, false);

                            if (LimitCheckMovement(limiteDeMovimento) && _moveButton.interactable)
                            {
                                playerGO.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject
                                    .SetActive(false);
                                move = true;
                                gameObject.GetComponent<Unit>().playSound(0);
                                _moveButton.interactable = false;
                            }
                            else
                            {
                                gameObject.GetComponent<Unit>().playSound(4);
                            }
                        }
                        else
                        {
                            Debug.Log("Hitting Nothing!");
                            gameObject.GetComponent<Unit>().playSound(4);
                        }
                    }
                }

                if (Input.GetButtonDown("Fire2") && myTurn)
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
    }

    private bool LimitCheckAttack(Vector2 posToGo)
    {
        var boolToReturn = false;
        var mathX = posToGo.x - playerGO.transform.position.x;        
        var mathY = posToGo.y - playerGO.transform.position.y;

        if (mathX == 1 || mathX == -1)
        {
            if (mathY == 0)
            {
                boolToReturn = true;
            }
        }
        else if(mathX <= 0.5 && mathX >= -0.5)
        {
            if (mathY <= 0.5 && mathY >= -0.5)
            {
                boolToReturn = true;
            }
        }
        
        return boolToReturn;
    }

    public void SetMyTurnToFalse()
    {
        myTurn = false;
    }

    public bool ReturnMyTurn()
    {
        return myTurn;
    }
    
    public void ActivateCommandsCanvas()
    {
        activateCommandsCanvas = true;
    }

    public void ActivateCommandsMenu()
    {
        var playerGo = gameObject.transform.GetChild(0);
        playerGo.transform.GetChild(0).gameObject.SetActive(false);
        playerGo.transform.GetChild(1).gameObject.SetActive(false);
        Commandos.SetActive(true);
    }

    public void DeactivateCommandsMenu()
    {
        Commandos.SetActive(false);
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

        var toCount = 0.25f;

        if (isX)
        {
            toCount = 0.5f;
        }
        else
        {
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
    
    private bool LimitCheckMovement(float value)
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
    
        Debug.Log("X2: " + x2 + " || VALUE: " + value);
        Debug.Log("Y2: " + y2 + " || VALUE: " + value);
        
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
    
    public void disableMoveButton(bool interac){
        _moveButton.interactable = interac;
    }

    public void setSkillCommandCanvas(bool boo){
        Debug.Log("SetSKillComandCanvas");
        ComandosSkills.SetActive(boo);      
    }

    public void DecreaseSkillsCD()
    {
        var allSkills = gameObject.GetComponents<Skill>();
        Debug.Log("ENTITY NAME: " + name);
        Debug.Log("SKILLS LENGTH: " + allSkills.Length);

        if (allSkills.Length > 0)
        {
            for (int x = 0; x < allSkills.Length; x++)
            {
                Debug.Log("SKILL[" + x + "] || NAME: " + allSkills[x].skillName + " || CAN USE SKILL: " + allSkills[x].ReturnCanUseSkill());
                
                if (!allSkills[x].SkillUsedThisTurn && !allSkills[x].ReturnCanUseSkill())
                {
                    allSkills[x].DecreaseCD();
                    Debug.Log("DECREASING SKILL COOLDOWN!");
                }
                else
                {
                    allSkills[x].SkillUsedThisTurn = false;
                }
            }
        }
    }
}