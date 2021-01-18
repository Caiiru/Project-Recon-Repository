using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onMouseOver : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;
    private GameObject tmap;
    public string tmapname;

    [SerializeField]private string tmapnametosend;
    private bool over;

    private GameObject player;

    void Start()
    {
        animator = this.GetComponent<Animator>();
        tmap = this.gameObject;
        
    }

    // Update is called once per frame
    void Update()
    {

        if(over == false){
            animator.SetBool("isOver",false);
        }

        if(Input.GetButtonDown("Fire1")){
            
            switch(tmapname){
                case "baixo":
                    if(over ==true){
                        tmapnametosend=tmapname;
                        MouseClick();
                    }
                break;
                case "cima":
                    if(over ==true){
                        tmapnametosend=tmapname;
                        MouseClick();
                    }
                break;
                case "direita":
                    if(over ==true){
                        tmapnametosend=tmapname;
                        MouseClick();
                    }
                break;
                case "esquerda":
                    if(over ==true){
                        tmapnametosend=tmapname;
                        MouseClick();
                    }
                break;

                default:
                    Debug.Log("pass");
                break;
            }
        }
    }

    public void OnMouseExit(){
       animator.SetBool("isOver",false);
        
        //over = false;
    }
    public void OnMouseEnter(){
       // animator.SetBool("isOver",true);
       // over = true;
        
    }
     void OnMouseOver(){
        //animator.SetBool("isOver",true);
        //over = true;
    }
    public void MouseClick(){
        if(GameObject.FindGameObjectWithTag("Player").GetComponent<Slash>().isAttacking == true){
            //GameObject.FindGameObjectWithTag("Player").GetComponent<Slash>().slashAttack(tmapnametosend);
            //Debug.Log(tmapnametosend);
        }
    }

    public void getAnimator(string name,bool boo){
        animator.SetBool(name,boo);
        over = true;
    }



    
}
