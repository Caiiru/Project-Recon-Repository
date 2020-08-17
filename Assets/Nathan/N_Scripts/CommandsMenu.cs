using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommandsMenu : MonoBehaviour
{
    private GameObject moveButton;

    private GameObject commandText;

    private EventSystem _eventSystem;
    
    void Start()
    {
        moveButton = GameObject.Find("MoveButton");
        commandText = GameObject.Find("CommandDesc");
        _eventSystem = EventSystem.current;
    }
    
    public void makeThePlayerMove(GameObject unityToMove)
    {
        
        unityToMove.GetComponent<battleWalk>().changeMoveBoolToTrue();
    }
}
