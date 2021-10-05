using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EnemyBattleWalk : MonoBehaviour
{
    public int limiteDeMovimento;

    public LayerMask targetLayerMask, WalkLayerMask;

    ////////////////////////////////////////////////////////////////////////////////
   
    private int _movementsMade;

    private bool _endTurn, _canAct, _targetSelected, _selectedPositionToMove;
    
    private List<GameObject> _enemies = new List<GameObject>();

    private GameObject _player, _companion1, _companion2, _target;
    
    private readonly bool[] _canMoveDirections = new bool[4];

    private readonly Vector3[] _allPositionsCheck = new Vector3[4];

    private Vector3 _goPos, _posToMoveTo;

    private TimerForTurn _timerForTurn;

    private EnemyParts _enemyParts;

    private EnemyAttackList _enemyAttackList;

    private bool moveTowardsTarget;

    private float x0, y0, x1, y1, x2, y2, x3, y3, x4, y4;
    
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _companion1 = GameObject.FindGameObjectWithTag("Companion1");
        _companion2 = GameObject.FindGameObjectWithTag("Companion2");
        _timerForTurn = gameObject.GetComponent<TimerForTurn>();
        _enemyParts = gameObject.GetComponent<EnemyParts>();
        _enemyAttackList = gameObject.GetComponent<EnemyAttackList>();
    }
    
    void Update()
    {
        _goPos = gameObject.transform.position;
        
        if (_canAct && _timerForTurn.Sinalizar())
        {            
            if (_targetSelected == false)
            {
                SelectNewTarget();
                _targetSelected = true;
                Debug.Log("TARGET SELECTED: " + _target.name);
                CheckCanMoveDirections();
            }
            else
            {
                for (int x = 0; x < _enemies.Count; x++)
                {
                    RaycastHit2D hit = Physics2D.Raycast(_enemies[x].transform.position, Vector3.back, Mathf.Infinity, targetLayerMask);

                    if (hit && hit.collider)
                    {
                        Debug.Log("I HIT: " + hit.transform.gameObject.name);
                        Debug.Log("OBJ TAG: " + hit.transform.gameObject.tag);
                    }

                    if (_selectedPositionToMove == false && hit && hit.collider && hit.collider.CompareTag("EnemyGrid"))
                    {
                        moveTowardsTarget = false;
                        SelectNewAttack();
                        break;
                    }
                    else
                    {
                        moveTowardsTarget = true;
                        Debug.Log("Setting monster to move!");
                    }
                }

                if (moveTowardsTarget)
                {
                    if (_movementsMade < limiteDeMovimento)
                    {
                        Move();
                    }
                    else
                    {
                        EndTurn();
                        Debug.Log("END");
                    }

                    if (Input.GetKeyDown(KeyCode.M) && _endTurn == false)
                    {
                        EndTurn();
                    }
                }
            }
        }
    }

    private void CheckCanMoveDirections()
    {
        ChangeFacingCoordinates();
        
        _allPositionsCheck[0] = new Vector3(_goPos.x + x0, _goPos.y + y0, _goPos.z); //ENEMYFRONT
        _allPositionsCheck[1] = new Vector3(_goPos.x + x1, _goPos.y + y1, _goPos.z); //ENEMYLEFT
        _allPositionsCheck[2] = new Vector3(_goPos.x + x2, _goPos.y + y2, _goPos.z); //ENEMYBACK
        _allPositionsCheck[3] = new Vector3(_goPos.x + x3, _goPos.y + y3, _goPos.z); //ENEMYRIGHT

        for(int x = 0; x < 4; x++)
        {
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(_allPositionsCheck[x].x, _allPositionsCheck[x].y), Vector3.forward, Mathf.Infinity, WalkLayerMask);
            Debug.DrawRay(_allPositionsCheck[x], Vector3.forward, Color.cyan, Mathf.Infinity);
            
            if (hit && hit.transform)
            {
                Debug.Log("I HIT: " + hit.transform.gameObject.name);
                Debug.Log("OBJ TAG: " + hit.transform.gameObject.tag);
            }

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
        moveTowardsTarget = false;
        _movementsMade = 0;
    }

    private void SelectNewTarget()
    {
        _enemies = new List<GameObject>();
        
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
        
        Debug.Log("Target selected: " + _target.name);

        for (int x = 0; x < _enemies.Count; x++)
        {
            Debug.Log("List pos[" + x + "]: " + _enemies[x].name);
        }
    }

    private void SelectNewAttack()
    {
        //_enemyAttackList.SelectNewAttackSpecific(0);
        _enemyAttackList.SelectNewAttack();
        Debug.Log("IM HOLDING AN ATTACK");
        EndTurn();
    }

    private void Move()
    {   
        if (_selectedPositionToMove == false)
        {
            Debug.Log("SELECTING POSITION");
            
            if (_target.transform.position.x > _goPos.x)
            {
                Debug.Log("TARGET X IS GREATER THAN ENEMY X");
                
                if (_target.transform.position.y >= _goPos.y)
                {
                    Debug.Log("TARGET Y IS GREATER THAN ENEMY Y");
                
                    if (_canMoveDirections[1])
                    {
                        Debug.Log("MOVING 1 (UP)");
                        _posToMoveTo = new Vector3(_goPos.x + 0.5f, _goPos.y + 0.25f, _goPos.z); //UP
                        _enemyParts.facingDirection = "UP";
                    }
                }
                else
                {
                    Debug.Log("TARGET Y IS SMALLER THAN ENEMY Y");
                    
                    Debug.Log(_canMoveDirections[0]);
                    
                    if (_canMoveDirections[0])
                    {
                        Debug.Log("MOVING 0 (RIGHT)");
                        _posToMoveTo = new Vector3(_goPos.x + 0.5f, _goPos.y - 0.25f, _goPos.z); //RIGHT
                        _enemyParts.facingDirection = "RIGHT";
                    }
                }
            }
            else
            {
                if (_target.transform.position.y >= _goPos.y)
                {
                    if (_canMoveDirections[2])
                    {
                        Debug.Log("MOVING 2 (LEFT)");
                        _posToMoveTo = new Vector3(_goPos.x - 0.5f, _goPos.y + 0.25f, _goPos.z); //LEFT
                        _enemyParts.facingDirection = "LEFT";
                    }
                }
                else
                {
                    if (_canMoveDirections[3])
                    {
                        Debug.Log("MOVING 3 (DOWN)");
                        _posToMoveTo = new Vector3(_goPos.x - 0.5f, _goPos.y - 0.25f, _goPos.z); //DOWN
                        _enemyParts.facingDirection = "DOWN";
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

    public GameObject ReturnTarget()
    {
        return _target;
    }

    public Unit[] ReturnEnemyParts()
    {
        return _enemyParts.ReturnAllEnemyParts();
    }

    public string ReturnFacingDirection()
    {
        return _enemyParts.facingDirection;
    }

    private void ChangeFacingCoordinates()
    {
        switch (_enemyParts.facingDirection)
        {
            case "RIGHT":
                x0 = 1;//FRONT
                y0 = -0.5f;//FRONT
                x1 = 1;//E_LEFT
                y1 = 0.5f;//E_LEFT
                x2 = -1.5f;//E_BACK
                y2 = 0.75f;//E_BACK
                x3 = -1;//E_RIGHT
                y3 = -0.5f;//E_RIGHT
                break;
            case "UP":
                x0 = 1;//FRONT
                y0 = 0.5f;//FRONT
                x1 = -1;//E_LEFT
                y1 = 0.5f;//E_LEFT
                x2 = -1.5f;//E_BACK
                y2 = -0.75f;//E_BACK
                x3 = 1;//E_RIGHT
                y3 = -0.5f;//E_RIGHT
                break;
            case "LEFT":
                x0 = -1;//FRONT
                y0 = 0.5f;//FRONT
                x1 = -1;//E_LEFT
                y1 = -0.5f;//E_LEFT
                x2 = 1.5f;//E_BACK
                y2 = -0.75f;//E_BACK
                x3 = 1;//E_RIGHT
                y3 = 0.5f;//E_RIGHT
                break;
            case "DOWN":
                x0 = -1;//FRONT
                y0 = -0.5f;//FRONT
                x1 = 1;//E_LEFT
                y1 = -0.5f;//E_LEFT
                x2 = 1.5f;//E_BACK
                y2 = 0.75f;//E_BACK
                x3 = -1;//E_RIGHT
                y3 = 0.5f;//E_RIGHT
                break;
        }
    }

    public List<GameObject> ReturnAllEnemiesList()
    {
        return _enemies;
    }
}