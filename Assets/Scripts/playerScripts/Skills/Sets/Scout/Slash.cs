using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash:Skill
{
   

    
    public GameObject SlashGO;

   
    private  GameObject slash1;
    private GameObject slash2;
    private GameObject slash3;
    private GameObject slash4;

    
    private Animator animator1;
    private Animator animator2;
    private Animator animator3;
    private Animator animator4;

   private LayerMask layerMask;
  

    public bool isAttacking = false;

    
   
    
 

    void Start(){
        
            //Instanciar os objetos e os filhos (Colliders e Marcadores)
            slash1 = SlashGO.transform.GetChild(0).gameObject; //baixo
            slash2 = SlashGO.transform.GetChild(1).gameObject; //esquerda
            slash3 = SlashGO.transform.GetChild(2).gameObject; //cima
            slash4 = SlashGO.transform.GetChild(3).gameObject; //direita
            animator1 = slash1.GetComponent<Animator>();
            animator2 = slash2.GetComponent<Animator>();
            animator3 = slash3.GetComponent<Animator>();
            animator4 = slash4.GetComponent<Animator>();
            layerMask = LayerMask.GetMask("Skill");
            
            
            
      
     
    }
    
    void Update(){
    Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    

    Debug.DrawRay(new Vector2(worldMousePosition.x,worldMousePosition.y),Vector3.forward,Color.cyan,Mathf.Infinity);
       
            if(isAttacking){
                if(Input.GetButtonDown("Fire2")) // Voltar o menu (Cancelar a skill)
                {
                    slash1.SetActive(false);
                    slash2.SetActive(false);
                    slash3.SetActive(false);
                    slash4.SetActive(false);
                    GameObject.FindGameObjectWithTag("Player").GetComponent<battleWalk>().setSkillCommandCanvas(true);
                    isAttacking = false;
                    animator1.SetBool("isOver",false);
                    animator2.SetBool("isOver",false);
                    animator3.SetBool("isOver",false);
                    animator4.SetBool("isOver",false);
                    
                }


                RaycastHit2D hit2D = Physics2D.Raycast(worldMousePosition,
                         Vector3.forward,Mathf.Infinity, layerMask);


                

                if(hit2D.collider.CompareTag("Skill")){
                    hit2D.collider.gameObject.GetComponent<onMouseOver>().getAnimator("isOver",true);
                }
               
                


                if(Input.GetButtonDown("Fire1")){
                    

    
                
            }
       }
      

        
            
    }

    public void Attack(){ //show range
        //Skill ativa
        slash1.SetActive(true);
        slash2.SetActive(true);
        slash3.SetActive(true);
        slash4.SetActive(true);
        isAttacking = true;
    
    }
   

   

    
    
    

    



}
