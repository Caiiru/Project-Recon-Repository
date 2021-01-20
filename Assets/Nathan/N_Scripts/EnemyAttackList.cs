using UnityEngine;
using System.Linq;
using Unity.Mathematics;
using UnityEngine.Tilemaps;

public class EnemyAttackList : MonoBehaviour
{
    public string enemyName;

    public LayerMask playerLayerMask, tileMaps;
    
    private EnemyBattleWalk _enemyBattleWalk;
    
    private EnemyAttack _attackSelected;
    
    private bool _holdingAttack;

    private int _turnsToHold, _turnsCont;
    
    public EnemyAttack[] allEnemyAttacks;
    
    private GameObject _target, _selectedLeg;

    private Vector3 _originPoint;

    private GameObject _attackPointer, _attackEffect;

    private void Start()
    {
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

    public void InitialAttack()
    {
        _target = _enemyBattleWalk.ReturnTarget();
        
        if (_attackSelected.attackType == "Ranged")
        {
            _originPoint = _target.transform.position;
        }
        else
        {
            if (enemyName == "DemoBoss")
            {
                float[] distanceToLegs = new float[4];
                
                Unit[] enemyParts = _enemyBattleWalk.ReturnEnemyParts();

                for (int x = 0; x < 4; x++)
                {
                    var pos = _target.transform.position - enemyParts[x].transform.position;
                    RaycastHit2D hit = Physics2D.Raycast(enemyParts[x].transform.position, pos, Mathf.Infinity, playerLayerMask);
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
        }
    }

    public void FinalAttack()
    {
        Destroy(_attackPointer);

        if (_attackSelected == allEnemyAttacks[0])
        {
            switch (_selectedLeg.name)
            {
                case "EnemyPart (2)":
                    _attackEffect = Instantiate(_attackSelected.afterEffect[0].gameObject, _originPoint, Quaternion.identity);
                break;
                case "EnemyPart (4)":
                    _attackEffect = Instantiate(_attackSelected.afterEffect[1].gameObject, _originPoint, Quaternion.identity);
                break;
                case "EnemyPart (1)":
                    _attackEffect = Instantiate(_attackSelected.afterEffect[2], _originPoint, Quaternion.identity);
                break;
                case "EnemyPart (3)":
                    _attackEffect = Instantiate(_attackSelected.afterEffect[3].gameObject, _originPoint, Quaternion.identity);
                break;
            }

            RaycastHit2D hit = Physics2D.Raycast(_target.transform.position, Vector3.forward, Mathf.Infinity, tileMaps);

            gameObject.GetComponent<Unit>().playSound(1);
            
            if (hit && hit.collider && hit.collider.CompareTag("EffectTilemap"))
            {
                _target.GetComponent<Unit>().TakeDamage((int) _attackSelected.damage, elements.NEUTRO);
                _target.GetComponent<Unit>().playSound(2);
            }

            Destroy(_attackEffect, 0.2f);
        }
       
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
}
