using TMPro;
using UnityEngine;

public class ShowNextUnitTurn : MonoBehaviour
{
    private Animator _animator;
    
    private battleSystem _battleSystem;

    public bool AnimationWasCompleted;

    private bool StartNextTurnAnimation, returnAnim, valuePrinted;
    
    // Start is called before the first frame update
    void Start()
    {
        _battleSystem = GameObject.Find("BattleSystem").GetComponent<battleSystem>();
        _animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (StartNextTurnAnimation)
        {
            /*
            if (!valuePrinted)
            {
                Debug.Log(_battleSystem.state.ToString());
                Debug.Log("ANIM BOOL:" + _animator.GetBool("showNextTurn"));
                Debug.Log("ANIM INT:" + _animator.GetInteger("entityID"));
                valuePrinted = true;
            }
            */
            if (_animator.GetBool("showNextTurn") && AnimationWasCompleted)
            {
                returnAnim = true;
            }
        }
    }

    public void StartNextTurnAnim()
    {
        StartNextTurnAnimation = true;
        
        switch (_battleSystem.state.ToString())
        {
            case "PLAYERTURN":
                _animator.SetInteger("entityID", 1);
                break;
            case "COMP1":
                _animator.SetInteger("entityID", 2);
                break;
            case "COMP2":
                _animator.SetInteger("entityID", 3);
                break;
            case "ENEMYTURN":
                _animator.SetInteger("entityID", 4);
                break;
        }
            
        _animator.SetBool("showNextTurn", true);
    }

    public void ResetReturnAnim()
    {
        returnAnim = false;
        StartNextTurnAnimation = false;
        valuePrinted = false;
    }
    
    public bool ReturnAnimFinished()
    {
        if (returnAnim)
        {
            _animator.SetBool("showNextTurn", false);
            _animator.SetInteger("entityID", 0);
        }
        
        return returnAnim;
    }
}
