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
    
    private bool _holdingAttack;

    private int _turnsToHold, _turnsCont;
    
    private GameObject _target, _selectedLeg;

    private Vector3 _originPoint;

    private GameObject _attackPointer, _attackEffect;

    private int legValue;

    private void Start()
    {
        enemyAttackPrioList = new List<EnemyAttack>();
        _enemyBattleWalk = gameObject.GetComponent<EnemyBattleWalk>();
    }

    public bool ReturnHoldingAttack()
    {
        return _holdingAttack;
    }

    public void ChangeHoldingAttack(bool toChangeTo)
    {
        _holdingAttack = toChangeTo;
    }

    public void SelectNewAttackSpecific(int attackIndex)
    {
        if (allEnemyAttacks[attackIndex] != null)
        {
            _attackSelected = allEnemyAttacks[attackIndex];
            _turnsToHold = _attackSelected.duration;
            _holdingAttack = true;
            InitialAttack();
        }
    }

    private void InitialAttack()
    {
        _target = _enemyBattleWalk.ReturnTarget();
        
        SpawnInitialEffectTilemap();
    }

    private void FinalAttack()
    {
        Destroy(_attackPointer);
              
        SpawnAfterEffectTilemap();
        
        RaycastHit2D hit = Physics2D.Raycast(_target.transform.position, Vector3.forward, Mathf.Infinity, tileMaps);

        gameObject.GetComponent<Unit>().playSound(1);
            
        if (hit && hit.collider && hit.collider.CompareTag("EffectTilemap"))
        {
            _target.GetComponent<Unit>().TakeDamage((int) _attackSelected.damage, elements.NEUTRO);
            _target.GetComponent<Unit>().playSound(2);
        }

        Destroy(_attackEffect, 0.2f);
       
        _holdingAttack = false;
    }
    
    public void AttackCheck()
    {
        if (_turnsCont >= _turnsToHold)
        {
            FinalAttack();
        }
    }
    
    public void AddToTurnsCount()
    {
        _turnsCont = _turnsCont + 1;
    }

    private void SpawnInitialEffectTilemap()
    {
        if (_attackSelected.attackType == "Ranged")
        {
            _originPoint = _target.transform.position;
        }
        else
        {
            if (enemyName == "DemoBoss")
            {
                if (_attackSelected.spawnPoint == "Legs")
                {
                    float[] distanceToLegs = new float[4];

                    Unit[] enemyParts = _enemyBattleWalk.ReturnEnemyParts();

                    for (int x = 0; x < 4; x++)
                    {
                        var pos = _target.transform.position - enemyParts[x].transform.position;
                        RaycastHit2D hit = Physics2D.Raycast(enemyParts[x].transform.position, pos, Mathf.Infinity,
                            playerLayerMask);
                        distanceToLegs[x] = hit.distance;
                        Debug.Log(enemyParts[x].name);
                        Debug.Log(hit.distance);
                    }

                    var lowestValue = distanceToLegs.Min();

                    for (int x = 0; x < 4; x++)
                    {
                        if (lowestValue == distanceToLegs[x])
                        {
                            _originPoint = enemyParts[x].transform.position;
                            _selectedLeg = enemyParts[x].gameObject;
                        }
                    }

                    Debug.Log(lowestValue);

                    _attackPointer = Instantiate(_attackSelected.gameObject, _originPoint, Quaternion.identity);

                    gameObject.GetComponent<Unit>().playSound(5);

                }
                else if (_attackSelected.spawnPoint == "Tail")
                {
                    Unit[] enemyParts = _enemyBattleWalk.ReturnEnemyParts();
                    _originPoint = enemyParts[5].transform.position;
                    _attackPointer = Instantiate(_attackSelected.gameObject, _originPoint, Quaternion.identity);
                    gameObject.GetComponent<Unit>().playSound(5);
                }
            }
        }
    }
    
    private void SpawnAfterEffectTilemap()
    {
        if (_attackSelected.gameObject.name == "StompAttackGrid")
        {
            SwitchSelectedLeg();
            
            _attackEffect = Instantiate(_attackSelected.afterEffect[legValue].gameObject, _originPoint, Quaternion.identity);
        }
        else
        {
            var dir = _enemyBattleWalk.ReturnFacingDirection();

            var tailDir = 1;
            
            if (dir == "LEFT" || dir == "DOWN")
            {
                tailDir = 0;
            }
            
            _attackEffect = Instantiate(_attackSelected.afterEffect[tailDir].gameObject, _originPoint, Quaternion.identity);
        }
    }

    public void SelectNewAttack()
    {
        Debug.Log("SelectNewAtttack");
        
        for (int x = 0; x < allEnemyAttacks.Length; x++)
        {
            enemyAttackPrioList.Add(allEnemyAttacks[x]);
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
        if (enemyAttackPrioList[0].gameObject.name.Contains("StompAttackGrid") ||
            enemyAttackPrioList[0].gameObject.name.Contains("TailSweepGrid"))
        {
            CheckForBossMeleeAttack();
        }
        
        _turnsToHold = _attackSelected.duration;
        _holdingAttack = true;
        InitialAttack();
    }

    private void CheckForBossMeleeAttack()
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
            RaycastHit2D hit = Physics2D.Raycast(enemyParts[x].transform.position, pos, Mathf.Infinity,
                playerLayerMask);
            distanceToParts[x] = hit.distance;
            Debug.Log(enemyParts[x].name);
            Debug.Log(hit.distance);
        }

        var lowestValue = distanceToParts.Min();

        for (int x = 0; x < 5; x++)
        {
            if (lowestValue == distanceToParts[x] && x >= 0 && x <= 3)
            {
                for (int y = 0; y < enemyAttackPrioList.Count; y++)
                {
                    if (enemyAttackPrioList[y].gameObject.name == "StompAttackGrid")
                    {
                        _attackSelected = enemyAttackPrioList[y];
                    }
                }
            }
            else if (lowestValue == distanceToParts[x] && x >= 4)
            {
                for (int y = 0; y < enemyAttackPrioList.Count; y++)
                {
                    if (_enemyBattleWalk.ReturnFacingDirection() == "LEFT" || _enemyBattleWalk.ReturnFacingDirection() == "RIGHT")
                    {
                        if (enemyAttackPrioList[y].gameObject.name == "TailSweepGrid(Left&Right)")
                        {
                            _attackSelected = enemyAttackPrioList[y];
                        }
                    }
                    else if(_enemyBattleWalk.ReturnFacingDirection() == "UP" || _enemyBattleWalk.ReturnFacingDirection() == "DOWN")
                    {
                        if (enemyAttackPrioList[y].gameObject.name == "TailSweepGrid(Up&Down)")
                        {
                            _attackSelected = enemyAttackPrioList[y];
                        }
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
                    legValue = 0;
                }
                else if (_selectedLeg.name == "EnemyPart (2)")
                {
                    legValue = 3;
                }
                else if (_selectedLeg.name == "EnemyPart (3)")
                {
                    legValue = 2;
                }
                else if (_selectedLeg.name == "EnemyPart (4)")
                {
                    legValue = 1;
                }
            break;
            case "UP":
                if (_selectedLeg.name == "EnemyPart (1)")
                {
                    legValue = 2;
                }
                else if (_selectedLeg.name == "EnemyPart (2)")
                {
                    legValue = 0;
                }
                else if (_selectedLeg.name == "EnemyPart (3)")
                {
                    legValue = 1;
                }
                else if (_selectedLeg.name == "EnemyPart (4)")
                {
                    legValue = 3;
                }
                break;
            case "LEFT":
                if (_selectedLeg.name == "EnemyPart (1)")
                {
                    legValue = 1;
                }
                else if (_selectedLeg.name == "EnemyPart (2)")
                {
                    legValue = 2;
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
            case "DOWN":
                if (_selectedLeg.name == "EnemyPart (1)")
                {
                    legValue = 3;
                }
                else if (_selectedLeg.name == "EnemyPart (2)")
                {
                    legValue = 1;
                }
                else if (_selectedLeg.name == "EnemyPart (3)")
                {
                    legValue = 0;
                }
                else if (_selectedLeg.name == "EnemyPart (4)")
                {
                    legValue = 2;
                }
                break;
        }
    }
}
