using UnityEngine;
using UnityEngine.Tilemaps;

public enum attackType { MELEE, RANGED }
    
public enum spawnPoint{ LEGS, TAIL, FRONT1BLOCK, FRONT3BLOCKS, FRONTPAWS}

public class EnemyAttack : MonoBehaviour
{
    public Tilemap attackTilemap;

    public GameObject[] afterEffect;
    
    public float damage;

    public int duration;

    public int priority;

    public spawnPoint _spawnPoint;

    public attackType _attackType;

    private bool _changedPrio;

    public void ChangePrioInt(string operation)
    {
        Debug.Log("CHANGEDPRIO: "+ _changedPrio);
        
        if (_changedPrio == false)
        {
            Debug.Log("IN CHANGED PRIO = FALSE");
            
            if (operation == "ADD")
            {
                Debug.Log("OLD NUMBER: " + priority);
                priority = priority + 1;
                Debug.Log("NEW NUMBER: " + priority);
            }
            else if (operation == "SUB")
            {
                Debug.Log("OLD NUMBER: " + priority);
                priority = priority - 1;
                Debug.Log("NEW NUMBER: " + priority);
            }
        }
        
        _changedPrio = true;
    }
    public void ChangePrio()
    {
        _changedPrio = false;
    }
}
