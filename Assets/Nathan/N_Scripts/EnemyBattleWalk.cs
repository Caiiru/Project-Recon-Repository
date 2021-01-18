using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EnemyBattleWalk : MonoBehaviour
{
    public int limiteDeMovimento;

    public LayerMask targetLayerMask;

    ////////////////////////////////////////////////////////////////////////////////
   
    private int _movementsMade, _attackSelected;

    private bool _holdingAttack, _endTurn, _canAct, _targetSelected, _selectedPositionToMove;
    
    private List<GameObject> _enemies = new List<GameObject>();

    private GameObject _player, _companion1, _companion2, _target;
    
    private readonly bool[] _canMoveDirections = new bool[4];

    private readonly Vector3[] _allPositionsCheck = new Vector3[4];

    private Vector3 _goPos, _posToMoveTo;

    private TimerForTurn _timerForTurn;

    private EnemyParts _enemyParts;
    
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _companion1 = GameObject.FindGameObjectWithTag("Companion1");
        _companion2 = GameObject.FindGameObjectWithTag("Companion2");
        _goPos = gameObject.transform.position;
        _timerForTurn = gameObject.GetComponent<TimerForTurn>();
        _enemyParts = gameObject.GetComponent<EnemyParts>();
    }
    
    void Update()
    {
        _goPos = gameObject.transform.position;
        
        if (_canAct && _timerForTurn.Sinalizar())
        {
            if (_holdingAttack == false)
            {
                if (_targetSelected == false)
                {
                    SelectNewTarget();
                    _targetSelected = true;
                    CheckCanMoveDirections();
                }
                else
                {
                    RaycastHit2D hit = Physics2D.Raycast(_target.transform.position, Vector3.back, Mathf.Infinity, targetLayerMask);

                    if (_selectedPositionToMove == false && hit && hit.collider && hit.collider.CompareTag("EnemyGrid"))
                    {
                        SelectNewAttack();
                    }
                    else
                    {
                        if (_movementsMade < limiteDeMovimento)
                        {
                            Move();
                        }
                        else
                        {
                            EndTurn();
                        }
                    }
                    
                    if (Input.GetKeyDown(KeyCode.M) && _endTurn == false)
                    {
                        EndTurn();
                    }
                }
            }
            else
            {
                Debug.Log("WAS HOLDING ATTACK");
                _holdingAttack = false;
            }
        }
    }

    private void CheckCanMoveDirections()
    {
        var allEnemyParts = _enemyParts.ReturnAllEnemyParts();
        
        _allPositionsCheck[0] = new Vector3(allEnemyParts[2].transform.position.x, _goPos.y - 0.75f, _goPos.z); //RIGHT
        _allPositionsCheck[1] = new Vector3(_goPos.x + 1.5f, _goPos.y + 0.75f, _goPos.z); //UP
        _allPositionsCheck[2] = new Vector3(_goPos.x - 2f, _goPos.y + 1f, _goPos.z); //LEFT
        _allPositionsCheck[3] = new Vector3(_goPos.x - 1.5f, _goPos.y - 0.75f, _goPos.z); //DOWN
        
        for(int x = 0; x < 4; x++)
        {
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(_allPositionsCheck[x].x, _allPositionsCheck[x].y), Vector3.forward, Mathf.Infinity);
            //Debug.DrawRay(_allPositionsCheck[x], Vector3.forward, Color.cyan, Mathf.Infinity);
            if (hit && hit.collider && hit.collider.CompareTag("Walk"))
            {
                _canMoveDirections[x] = true;
            }
            else
            {
                _canMoveDirections[x] = false;
            }
            Debug.Log("DIR" + x + " BOOL: " +_canMoveDirections[x]);
        }
    }

    public bool ReturnEndTurn()
    {
        return _endTurn;
    }

    public void ChangeEndTurn(bool toChangeTo)
    {
        _endTurn = toChangeTo;
    }
    
    public void ChangeCanAct(bool toChangeTo)
    {
        _canAct = toChangeTo;
        if (_canAct)
        {
            _timerForTurn.Iniciar(1.5f);
        }
    }

    private void EndTurn()
    {
        _endTurn = true;
        _canAct = false;
        _targetSelected = false;
        _movementsMade = 0;
    }

    private void SelectNewTarget()
    {
        _enemies.Add(_player);
        
        if(_companion1 != null){
            _enemies.Add(_companion1);
        }
        
        if(_companion2 != null){
            _enemies.Add(_companion2);
        }

        _enemies = _enemies.OrderBy(e => e.GetComponent<Unit>().AgressionNumber).ToList();
        _enemies.Reverse();

        _target = _enemies[0];
    }

    private void SelectNewAttack()
    {
        _attackSelected = 0;
        _holdingAttack = true;
        Debug.Log("IM HOLDING AN ATTACK");
        EndTurn();
    }

    private void Move()
    {
        if (_selectedPositionToMove == false)
        {
            if (_target.transform.position.x > _goPos.x)
            {
                if (_target.transform.position.y >= _goPos.y)
                {
                    if (_canMoveDirections[1])
                    {
                        _posToMoveTo = new Vector3(_goPos.x + 0.5f, _goPos.y + 0.25f, _goPos.z); //UP
                    }
                }
                else
                {
                    if (_canMoveDirections[0])
                    {
                        _posToMoveTo = new Vector3(_goPos.x + 0.5f, _goPos.y - 0.25f, _goPos.z); //RIGHT
                    }
                }
            }
            else
            {
                if (_target.transform.position.y >= _goPos.y)
                {
                    if (_canMoveDirections[2])
                    {
                        _posToMoveTo = new Vector3(_goPos.x - 0.5f, _goPos.y + 0.25f, _goPos.z); //LEFT
                    }
                }
                else
                {
                    if (_canMoveDirections[3])
                    {
                        _posToMoveTo = new Vector3(_goPos.x - 0.5f, _goPos.y - 0.25f, _goPos.z); //DOWN
                    }
                }
            }

            _selectedPositionToMove = true;
        }
        else
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, _posToMoveTo, 0.0225f);
            if (gameObject.transform.position == _posToMoveTo)
            {
                _movementsMade++;
                _selectedPositionToMove = false;
                _timerForTurn.Reiniciar();
                _timerForTurn.Iniciar(1.5f);
            }
        }
    }
}