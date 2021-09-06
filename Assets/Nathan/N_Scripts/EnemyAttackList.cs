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
            allEnemyAttacks[x].ChangePrioNumberToZero();
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
            
            Debug.Log("NameCheck: " + nameCheck);
            
            if (allEnemyAttacks[x] == _attackSelected)
            {
                allEnemyAttacks[x].ChangePrioInt("SUB");
                Debug.Log("SUBTRACTED FROM ATTACK SELECTED");
            }
            else
            {
                if (allEnemyAttacks[x].gameObject.name.Contains(nameCheck) && nameCheck != "")
                {
                    allEnemyAttacks[x].ChangePrioInt("SUB");
                    Debug.Log("SUBTRACTED FROM ATTACK BROTHER");
                }
                else
                {
                    allEnemyAttacks[x].ChangePrioInt("ADD");
                    Debug.Log("ADDED TO OTHER ATTACK");
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
            allEnemyAttacks[x].ChangePrio();
        }
        
        Debug.Log("SelectNewAtttack");

        if (enemyAttackPrioList.Count == 0)
        {
            for (int x = 0; x < allEnemyAttacks.Length; x++)
            {
                enemyAttackPrioList.Add(allEnemyAttacks[x]);
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
        var attackname = enemyAttackPrioList[0].gameObject.name;
        
        if (attackname.Contains("Tremor") || attackname.Contains("Horn") || attackname.Contains("Canon"))
        {
            Debug.Log("IN TREMOR ATTACK");
            
            _attackSelected = enemyAttackPrioList[0];
        }
        else
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
                            _attackSelected = enemyAttackPrioList[y];
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
                            _attackSelected = enemyAttackPrioList[y];
                        }
                    }
                }
            }
        }
        
        Debug.Log(_attackSelected.name);
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
