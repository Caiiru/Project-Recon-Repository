using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash:Skill
{
    private GameObject enemyGO;

    private Collider2D col;
    public GameObject SlashGO;

    private GameObject playerGO;
   
    private  GameObject slash1;
    private GameObject slash2;
    private GameObject slash3;
    private GameObject slash4;

    
    private Animator animator1;
    private Animator animator2;
    private Animator animator3;
    private Animator animator4;

   private LayerMask layerMask;
   public LayerMask layermask2;

    public bool isAttacking = false;

    
    [SerializeField] bool isCollider;
    
    [SerializeField] GameObject p;

    void Start(){
        if(!isCollider){
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
            
            
            enemyGO = GameObject.FindGameObjectWithTag("Enemy").transform.gameObject;
            col = enemyGO.GetComponent<Collider2D>();
            playerGO = GameObject.FindGameObjectWithTag("Player").transform.gameObject;
        }
        else{
            SlashGO = GameObject.FindGameObjectWithTag("Player").transform.GetChild(1).GetChild(0).gameObject;
        }
    }
    
    void Update(){
    Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    

    Debug.DrawRay(new Vector2(worldMousePosition.x,worldMousePosition.y),Vector3.forward,Color.cyan,Mathf.Infinity);
       if(!isCollider){
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
                    col.enabled = true;
                }


                RaycastHit2D hit2D = Physics2D.Raycast(worldMousePosition,
                         Vector3.forward,Mathf.Infinity, layerMask);


                RaycastHit2D ray = Physics2D.Raycast(worldMousePosition,Vector3.forward,Mathf.Infinity,layermask2);
                

                if(hit2D.collider.CompareTag("Skill")){
                    hit2D.collider.gameObject.GetComponent<onMouseOver>().getAnimator("isOver",true);
                }
               
                
                else {
                    //GameObject.FindGameObjectWithTag("Skill").transform.gameObject.GetComponent<onMouseOver>().getAnimator("isOver",false);
                    p = GameObject.FindGameObjectWithTag("Player").transform.gameObject;
                    p.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<onMouseOver>().getAnimator("isOver",false);
                    p.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).GetComponent<onMouseOver>().getAnimator("isOver",false);  
                    p.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.transform.GetChild(2).GetComponent<onMouseOver>().getAnimator("isOver",false);  
                    p.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.transform.GetChild(3).GetComponent<onMouseOver>().getAnimator("isOver",false);      
                }

                

                if(Input.GetButtonDown("Fire1")){
                    Vector3Int tilecoord = playerGO.GetComponent<battleWalk>().map.WorldToCell(worldMousePosition);
                    Vector2 Cellcenterpos = playerGO.GetComponent<battleWalk>().map.GetCellCenterWorld(tilecoord);

                    
                   

                    Debug.Log(hit2D.collider.gameObject.tag);
                    if(ray.collider){
                        if(ray.collider.CompareTag("Enemy") || hit2D.collider.CompareTag("EnemyPart")){
                            Debug.Log("Hit Enemy");
                        }
                    }
                }
                
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
        col.enabled = false;
    }
    private void hideRange(){
                slash1.SetActive(false);
                slash2.SetActive(false);
                slash3.SetActive(false);
                slash4.SetActive(false);
                
                isAttacking = false;
                animator1.SetBool("isOver",false);
                animator2.SetBool("isOver",false);
                animator3.SetBool("isOver",false);
                animator4.SetBool("isOver",false);
    }

    public void slashAttack(string side){
        hideRange();
        switch(side){
            case "baixo":
                
                break;
            case "cima":
                    
                break;
            case "esquerda":
                    
                break;
            case "direita":
                    
                break;


             default:
                Debug.Log("erro");
                break;
        }
    
    }

    private void checkRange(){
        if(GameObject.FindGameObjectWithTag("Enemy") ){

        }

    }

    
    

    



}
