using UnityEngine;

public class CommandsMenu : MonoBehaviour
{   
    public void MakeThePlayerMove(GameObject unityToMove)
    {
        unityToMove.GetComponent<battleWalk>().ChangeMoveBool(true);
    }
}
