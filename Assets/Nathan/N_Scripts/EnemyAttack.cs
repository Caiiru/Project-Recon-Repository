using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float damage;

    public int priority;

    private bool _changedPrio;

    public GameObject[] attackGrids;
    
    public elements AttackElement;

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

    public void ChangePrioNumberToZero()
    {
        priority = 0;
    }
}
