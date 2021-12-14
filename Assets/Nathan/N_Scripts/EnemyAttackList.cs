using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using UnityEngine;
using System.Linq;
using Unity.Mathematics;
using UnityEngine.Tilemaps;

public class EnemyAttackList : MonoBehaviour
{
    public string enemyName;

    public LayerMask playerLayerMask, tileMaps;
    
    public EnemyAttack[] allEnemyAttacks;

    private List<EnemyAttack> enemyAttackPrioList;
    
    private EnemyBattleWalk _enemyBattleWalk;
    
    private EnemyAttack _attackSelected;
    
    private GameObject _target, _selectedLeg;

    private Vector3 _originPoint;

    private GameObject _attackEffect;

    private int legValue;

    private RaycastHit2D hit;

    private void Start()
    {
        enemyAttackPrioList = new List<EnemyAttack>();
        _enemyBattleWalk = gameObject.GetComponent<EnemyBattleWalk>();

        for (int x = 0; x < allEnemyAttacks.Length; x++)
        {
            if (allEnemyAttacks[x] != null)
            {
                allEnemyAttacks[x].ChangePrioNumberToZero();
            }
        }
    }
    
    private void InitiateAttack()
    {
        Debug.Log("FINAL ATTACK");
              
        Debug.Log("ATTACK SELECTED NAME: " + _attackSelected.name);
        
        SpawnEffectTilemap();
        
        var enemiesList = _enemyBattleWalk.ReturnAllEnemiesList();
        
        for (int x = 0; x < enemiesList.Count; x++)
        {
            RaycastHit2D hit = Physics2D.Raycast(enemiesList[x].transform.position, Vector3.forward, Mathf.Infinity, tileMaps);

            gameObject.GetComponent<Unit>().playSound(1);

            if (hit && hit.collider && hit.collider.CompareTag("EffectTilemap"))
            {
                enemiesList[x].GetComponent<Unit>().TakeDamage((int) _attackSelected.damage, _attackSelected.AttackElement);
                if (_attackSelected.AttackEffect != 0)
                {
                    enemiesList[x].GetComponent<Unit>().AddStatusEffect(_attackSelected.AttackEffect);
                }
                enemiesList[x].GetComponent<Unit>().playSound(2);
                BattleRating.DamageTaken = BattleRating.DamageTaken - (int) _attackSelected.damage;
            }
        }

        Destroy(_attackEffect, 0.2f);
        
        for (int x = 0; x < allEnemyAttacks.Length; x++)
        {
            var nameCheck = _attackSelected.gameObject.name;
            
            Debug.Log("SELECTED ATTACK NAME: " + nameCheck);
            
            Debug.Log("ALL ENEMY ATTACKS ["+x+"] NAME: " + allEnemyAttacks[x].name);
            
            if (allEnemyAttacks[x] != null && allEnemyAttacks[x].name == _attackSelected.name)
            {
                Debug.Log("SUBTRACTED FROM ATTACK SELECTED");
                allEnemyAttacks[x].ChangePrioInt("SUB");
            }
            else
            {
                if (allEnemyAttacks[x] != null)
                {
                    if (allEnemyAttacks[x].gameObject.name.Contains(nameCheck) && nameCheck != "")
                    {
                        Debug.Log("SUBTRACTED FROM ATTACK BROTHER");
                        allEnemyAttacks[x].ChangePrioInt("SUB");
                    }
                    else
                    {
                        Debug.Log("ADDED TO OTHER ATTACK");
                        allEnemyAttacks[x].ChangePrioInt("ADD");
                    }
                }
            }
        }
        
        Debug.Log("FINISHED FINAL ATTACK");
    }
    
    private void SpawnEffectTilemap()
    {
        if (_attackSelected.gameObject.name == "StompAttack")
        {
            SwitchSelectedLeg();
            
            _attackEffect = Instantiate(_attackSelected.attackGrids[legValue], _selectedLeg.transform.position, Quaternion.identity);
        }
        else if(_attackSelected.gameObject.name.Contains("TailSweep"))
        {
            var gridIndex = 0;
            
            var enemyTail = _enemyBattleWalk.ReturnEnemyParts()[5];
            
            var dir = _enemyBattleWalk.ReturnFacingDirection();

            switch (dir)
            {
                case "UP":
                    gridIndex = 1;
                break;
                case "LEFT":
                    gridIndex = 2;
                break;
                case "DOWN":
                    gridIndex = 3;
                break;
            }
            
            _attackEffect = Instantiate(_attackSelected.attackGrids[gridIndex], enemyTail.transform.position, Quaternion.identity);
        }
        else if(_attackSelected.gameObject.name.Contains("HornShot"))
        {
            var dirInt = 0;
            _originPoint = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
            var diffX = 1;
            var diffY = 0.5f;
            switch (_enemyBattleWalk.ReturnFacingDirection())
            {
                case "RIGHT":
                    dirInt = 0;
                    diffY = diffY * -1;
                break;
                case "UP":
                    dirInt = 1;
                break;
                case "LEFT":
                    dirInt = 2;
                    diffX = diffX * -1;
                break;
                case "DOWN":
                    dirInt = 3;
                    diffX = diffX * -1;
                    diffY = diffY * -1;
                break;
            }       
            _originPoint = new Vector3(_originPoint.x + diffX, _originPoint.y + diffY, _originPoint.z);
            _attackEffect = Instantiate(_attackSelected.attackGrids[dirInt], _originPoint, Quaternion.identity);
        }
        else if(_attackSelected.gameObject.name.Contains("Tremor"))
        {
            _originPoint = gameObject.transform.position;
            
            var dir = _enemyBattleWalk.ReturnFacingDirection();

            switch (dir)
            {
                case "RIGHT":
                    var newPos = new Vector3(_originPoint.x + 1f, _originPoint.y - 0.5f, _originPoint.z);
                    _attackEffect = Instantiate(_attackSelected.attackGrids[0], newPos, Quaternion.identity);
                break;
                case "UP":
                    newPos = new Vector3(_originPoint.x + 1f, _originPoint.y +0.5f, _originPoint.z);
                    _attackEffect = Instantiate(_attackSelected.attackGrids[1], newPos, Quaternion.identity);
                break;
                case "LEFT":
                    newPos = new Vector3(_originPoint.x - 1f, _originPoint.y + 0.5f, _originPoint.z);
                    _attackEffect = Instantiate(_attackSelected.attackGrids[2], newPos, Quaternion.identity);
                break;
                case "DOWN":
                    newPos = new Vector3(_originPoint.x - 1f, _originPoint.y - 0.5f, _originPoint.z);
                    _attackEffect = Instantiate(_attackSelected.attackGrids[3], newPos, Quaternion.identity);
                break;
            }
        }
        else
        {
            _attackEffect = Instantiate(_attackSelected.attackGrids[0], ChangeAttackPositionInFront(), Quaternion.identity);
        }
    }

    public void SelectNewAttack()
    {
        for (int x = 0; x < allEnemyAttacks.Length; x++)
        {
            if (allEnemyAttacks[x] != null)
            {
                allEnemyAttacks[x].ChangePrio();
            }
        }
        
        Debug.Log("SelectNewAtttack");

        if (enemyAttackPrioList.Count == 0)
        {
            for (int x = 0; x < allEnemyAttacks.Length; x++)
            {
                if (allEnemyAttacks[x] != null)
                {
                    enemyAttackPrioList.Add(allEnemyAttacks[x]);
                }
            }
        }

        enemyAttackPrioList = enemyAttackPrioList.OrderBy(e => e.GetComponent<EnemyAttack>().priority).ToList();
        enemyAttackPrioList.Reverse();

        foreach (var x in enemyAttackPrioList)
        {
            Debug.Log(x.name);
        }
        
        StrategyCheck();
    }

    private void StrategyCheck()
    {   
        CheckForBossMeleeAttack();
        
        InitiateAttack();
    }

    private void CheckForBossMeleeAttack()
    {        
        var breakDetectionLoop = false;
        
        for (int z = 0; z < enemyAttackPrioList.Count; z++)
        {
            Debug.Log("FOR INDEX ["+z+"]");
            Debug.Log("ENEMYATTACKPRIOLISTNAME: " + enemyAttackPrioList[z].name);

            if (enemyAttackPrioList[z].name.Contains("Stomp") || enemyAttackPrioList[z].name.Contains("Tail"))
            {
                Debug.Log("CHECKING FOR STOMP OR TAIL ATTACK!!!");
                
                CheckForLegAttack();
                
                SwitchSelectedLeg();

                Debug.Log("SELECTED A LEG FOR THE ATTACK!!!");
                
                var _effectGrid = Instantiate(enemyAttackPrioList[z].attackGrids[legValue],
                    _selectedLeg.transform.position,
                    Quaternion.identity);
                
                Debug.Log("EFFECT GRID: " + _effectGrid.name);

                var _effectTilemap = _effectGrid.transform.GetChild(0).gameObject;
                
                Debug.Log("EFFECT TILEMAP: " + _effectTilemap.name);

                var _tilemapColor = _effectTilemap.GetComponent<Tilemap>().color;

                _tilemapColor = new Color(0, 0, 0, 0);

                _effectTilemap.GetComponent<Tilemap>().color = _tilemapColor;

                var allenemies = _enemyBattleWalk.ReturnAllEnemiesList();
                
                Debug.Log("TARGETS CLOSE: " + allenemies.Count);

                for (int y = 0; y < allenemies.Count; y++)
                {
                    RaycastHit2D hit2 = Physics2D.Raycast(allenemies[y].transform.position, Vector3.back, Mathf.Infinity, tileMaps);
                    Debug.DrawRay(allenemies[y].transform.position, Vector3.back, Color.green,Mathf.Infinity);
                    
                    Debug.Log("RAY COMING FROM: " + allenemies[y].name);
                    Debug.Log("TargetsIndex: " + y);
                        
                    if (hit2.collider && hit2.transform.gameObject == _effectTilemap)
                    {
                        Debug.Log("I HIT: " + hit2.transform.gameObject.name);
                        Debug.Log("OBJ TAG: " + hit2.transform.gameObject.tag);
                        Debug.Log("_attackSelected was Set!");
                        _attackSelected = enemyAttackPrioList[z];
                        breakDetectionLoop = true;
                        Destroy(_effectGrid);
                        break;
                    }
                }

                Destroy(_effectGrid);
            }
            else if(enemyAttackPrioList[z].name.Contains("Horn"))
            {
                Debug.Log("CHECKING FOR HORN ATTACK!!!!!");
             
                var spawnpoint = gameObject.transform.position;
                
                var dirInt = 0;
                var diffX = 1;
                var diffY = 0.5f;
                
                switch (_enemyBattleWalk.ReturnFacingDirection())
                {
                    case "RIGHT":
                        dirInt = 0;
                        diffY = diffY * -1;
                        break;
                    case "UP":
                        dirInt = 1;
                        break;
                    case "LEFT":
                        dirInt = 2;
                        diffX = diffX * -1;
                        break;
                    case "DOWN":
                        dirInt = 3;
                        diffX = diffX * -1;
                        diffY = diffY * -1;
                        break;
                }
                
                spawnpoint = new Vector3(spawnpoint.x + diffX, spawnpoint.y + diffY, spawnpoint.z);

                var _effectGrid = Instantiate(enemyAttackPrioList[z].attackGrids[dirInt], spawnpoint, Quaternion.identity);
                
                Debug.Log("EFFECT GRID: " + _effectGrid.name);

                var _effectTilemap = _effectGrid.transform.GetChild(0).gameObject;
                
                Debug.Log("EFFECT TILEMAP: " + _effectTilemap.name);

                var _tilemapColor = _effectTilemap.GetComponent<Tilemap>().color;

                _tilemapColor = new Color(0, 0, 0, 0);

                _effectTilemap.GetComponent<Tilemap>().color = _tilemapColor;

                var allenemies = _enemyBattleWalk.ReturnAllEnemiesList();
                
                for (int y = 0; y < allenemies.Count; y++)
                {
                    RaycastHit2D hit2 = Physics2D.Raycast(allenemies[y].transform.position, Vector3.back, Mathf.Infinity, tileMaps);
                    Debug.DrawRay(allenemies[y].transform.position, Vector3.forward, Color.green,Mathf.Infinity);
                    
                    Debug.Log("RAY COMING FROM: " + allenemies[y].name);
                    Debug.Log("TargetsIndex: " + y);
                    
                    if (hit2 && hit2.collider && hit2.transform.gameObject == _effectTilemap)
                    {
                        Debug.Log("I HIT: " + hit2.transform.gameObject.name);
                        Debug.Log("OBJ TAG: " + hit2.transform.gameObject.tag);
                        Debug.Log("_attackSelected was Set!");
                        _attackSelected = enemyAttackPrioList[z];
                        breakDetectionLoop = true;
                        Destroy(_effectGrid);
                        break;
                    }
                }

                Destroy(_effectGrid);          
            }
            else if (enemyAttackPrioList[z].name.Contains("Tremor"))
            {
                var spawnpoint = gameObject.transform.position;

                var dir = _enemyBattleWalk.ReturnFacingDirection();

                var dirIndex = 0;
                
                switch (dir)
                {
                    case "RIGHT":
                        spawnpoint = new Vector3(spawnpoint.x + 1f, spawnpoint.y - 0.5f, spawnpoint.z);
                        break;
                    case "UP":
                        spawnpoint = new Vector3(spawnpoint.x + 1f, spawnpoint.y + 0.5f, spawnpoint.z);
                        dirIndex = 1;
                        break;
                    case "LEFT":
                        spawnpoint = new Vector3(spawnpoint.x - 1f, spawnpoint.y + 0.5f, spawnpoint.z);
                        dirIndex = 2;
                        break;
                    case "DOWN":
                        spawnpoint = new Vector3(spawnpoint.x - 1f, spawnpoint.y - 0.5f, spawnpoint.z);
                        dirIndex = 3;
                        break;
                }

                var _effectGrid = Instantiate(enemyAttackPrioList[z].attackGrids[dirIndex], spawnpoint, Quaternion.identity);
                
                Debug.Log("EFFECT GRID: " + _effectGrid.name);

                var _effectTilemap = _effectGrid.transform.GetChild(0).gameObject;
                
                Debug.Log("EFFECT TILEMAP: " + _effectTilemap.name);

                var _tilemapColor = _effectTilemap.GetComponent<Tilemap>().color;

                _tilemapColor = new Color(0, 0, 0, 0);

                _effectTilemap.GetComponent<Tilemap>().color = _tilemapColor;

                var allenemies = _enemyBattleWalk.ReturnAllEnemiesList();
                
                for (int y = 0; y < allenemies.Count; y++)
                {
                    RaycastHit2D hit2 = Physics2D.Raycast(allenemies[y].transform.position, Vector3.back, Mathf.Infinity, tileMaps);
                    Debug.DrawRay(allenemies[y].transform.position, Vector3.forward, Color.green,Mathf.Infinity);
                    
                    Debug.Log("RAY COMING FROM: " + allenemies[y].name);
                    Debug.Log("TargetsIndex: " + y);
                    
                    if (hit2 && hit2.collider && hit2.transform.gameObject == _effectTilemap)
                    {
                        Debug.Log("I HIT: " + hit2.transform.gameObject.name);
                        Debug.Log("OBJ TAG: " + hit2.transform.gameObject.tag);
                        Debug.Log("_attackSelected was Set!");
                        _attackSelected = enemyAttackPrioList[z];
                        breakDetectionLoop = true;
                        Destroy(_effectGrid);
                        break;
                    }
                }

                Destroy(_effectGrid);  
            }
            else
            {
                var _effectGrid = Instantiate(enemyAttackPrioList[z].attackGrids[0], ChangeAttackPositionInFront(), Quaternion.identity);
                
                Debug.Log("EFFECT GRID: " + _effectGrid.name);

                var _effectTilemap = _effectGrid.transform.GetChild(0).gameObject;
                
                Debug.Log("EFFECT TILEMAP: " + _effectTilemap.name);

                var _tilemapColor = _effectTilemap.GetComponent<Tilemap>().color;

                _tilemapColor = new Color(0, 0, 0, 0);

                _effectTilemap.GetComponent<Tilemap>().color = _tilemapColor;

                var allenemies = _enemyBattleWalk.ReturnAllEnemiesList();
                
                for (int y = 0; y < allenemies.Count; y++)
                {
                    RaycastHit2D hit2 = Physics2D.Raycast(allenemies[y].transform.position, Vector3.back, Mathf.Infinity, tileMaps);
                    Debug.DrawRay(allenemies[y].transform.position, Vector3.forward, Color.green,Mathf.Infinity);
                    
                    Debug.Log("RAY COMING FROM: " + allenemies[y].name);
                    Debug.Log("TargetsIndex: " + y);
                    
                    if (hit2 && hit2.collider && hit2.transform.gameObject == _effectTilemap)
                    {
                        Debug.Log("I HIT: " + hit2.transform.gameObject.name);
                        Debug.Log("OBJ TAG: " + hit2.transform.gameObject.tag);
                        Debug.Log("_attackSelected was Set!");
                        _attackSelected = enemyAttackPrioList[z];
                        breakDetectionLoop = true;
                        Destroy(_effectGrid);
                        break;
                    }
                }

                Destroy(_effectGrid);  
            }
            
            if (breakDetectionLoop)
            {
                Debug.Log("Breaking dectection loop!");
                break;
            }
        }
            
        Debug.Log(_attackSelected.name);
    }

    private void CheckForLegAttack()
    {
        float[] distanceToParts = new float[5];

        Unit[] enemyParts = _enemyBattleWalk.ReturnEnemyParts();
        
        for (int x = 0; x < 5; x++)
        {
            if (_target == null)
            {
                _target = _enemyBattleWalk.ReturnTarget();
            }
                
            var pos = _target.transform.position - enemyParts[x].transform.position;
            hit = Physics2D.Raycast(enemyParts[x].transform.position, pos, Mathf.Infinity, playerLayerMask);
            Debug.DrawRay(enemyParts[x].transform.position, pos, Color.yellow,Mathf.Infinity);
            distanceToParts[x] = hit.distance;
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("HIT PLAYER COLLIDER!");
                Debug.Log("DISTANCE: " + hit.distance);
                Debug.Log("LEG: " + enemyParts[x].name);
            }
            else
            {
                Debug.Log("DIDNT HIT TARGET!");
            }
        }
        
        var lowestValue = distanceToParts.Min();

        for (int x = 0; x < 5; x++)
        {
            if (lowestValue == distanceToParts[x] && x >= 0 && x <= 3)
            {
                for (int y = 0; y < enemyAttackPrioList.Count; y++)
                {
                    if (enemyAttackPrioList[y].gameObject.name == "StompAttack")
                    {
                        //_attackSelected = enemyAttackPrioList[y];
                        _selectedLeg = enemyParts[x].gameObject;
                        Debug.Log("SELECTED LEG NAME: " + _selectedLeg.name);
                    }
                }
            }
            else if (lowestValue == distanceToParts[x] && x >= 4)
            {
                for (int y = 0; y < enemyAttackPrioList.Count; y++)
                {
                    if (enemyAttackPrioList[y].gameObject.name == "TailSweepAttack")
                    {
                        _selectedLeg = enemyParts[4].gameObject;
                        //_attackSelected = enemyAttackPrioList[y];
                    }
                }
            }
        }
    }
    
    private void SwitchSelectedLeg()
    {
        switch (_enemyBattleWalk.ReturnFacingDirection())
        {
            case "RIGHT":
                if (_selectedLeg.name == "EnemyPart (1)")
                {
                    legValue = 1;
                }
                else if (_selectedLeg.name == "EnemyPart (2)")
                {
                    legValue = 0;
                }
                else if (_selectedLeg.name == "EnemyPart (3)")
                {
                    legValue = 2;
                }
                else if (_selectedLeg.name == "EnemyPart (4)")
                {
                    legValue = 3;
                }
            break;
            case "UP":
                if (_selectedLeg.name == "EnemyPart (1)")
                {
                    legValue = 2;
                }
                else if (_selectedLeg.name == "EnemyPart (2)")
                {
                    legValue = 1;
                }
                else if (_selectedLeg.name == "EnemyPart (3)")
                {
                    legValue = 3;
                }
                else if (_selectedLeg.name == "EnemyPart (4)")
                {
                    legValue = 0;
                }
                break;
            case "LEFT":
                if (_selectedLeg.name == "EnemyPart (1)")
                {
                    legValue = 3;
                }
                else if (_selectedLeg.name == "EnemyPart (2)")
                {
                    legValue = 2;
                }
                else if (_selectedLeg.name == "EnemyPart (3)")
                {
                    legValue = 0;
                }
                else if (_selectedLeg.name == "EnemyPart (4)")
                {
                    legValue = 1;
                }
                break;
            case "DOWN":
                if (_selectedLeg.name == "EnemyPart (1)")
                {
                    legValue = 0;
                }
                else if (_selectedLeg.name == "EnemyPart (2)")
                {
                    legValue = 3;
                }
                else if (_selectedLeg.name == "EnemyPart (3)")
                {
                    legValue = 1;
                }
                else if (_selectedLeg.name == "EnemyPart (4)")
                {
                    legValue = 2;
                }
                break;
        }
    }

    private Vector3 ChangeAttackPositionInFront()
    {
        var vector3pos = new Vector3(gameObject.transform.position.x,gameObject.transform.position.y,gameObject.transform.position.z);

        var dir = _enemyBattleWalk.ReturnFacingDirection();

        switch (dir)
        {
            case "RIGHT":
                var xPos = vector3pos.x + 2f;
                var yPos = vector3pos.y - 1f;
                vector3pos = new Vector3(xPos, yPos, vector3pos.z);
            break;
            case "LEFT":
                xPos = vector3pos.x - 2f;
                yPos = vector3pos.y + 1f;
                vector3pos = new Vector3(xPos, yPos, vector3pos.z);
            break;
            case "UP":
                xPos = vector3pos.x + 2f;
                yPos = vector3pos.y + 1f;
                vector3pos = new Vector3(xPos, yPos, vector3pos.z);
            break;
            case "DOWN":
                xPos = vector3pos.x - 2f;
                yPos = vector3pos.y - 1f;
                vector3pos = new Vector3(xPos, yPos, vector3pos.z);
            break;
        }
                    
        return vector3pos;
    }
}
