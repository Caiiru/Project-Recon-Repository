using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : Skill
{
    
    public GameObject SlashGO;


    private GameObject slash1;
    private GameObject slash2;
    private GameObject slash3;
    private GameObject slash4;


    private Animator animator1;
    private Animator animator2;
    private Animator animator3;
    private Animator animator4;

    public LayerMask layerMask;
   

    public bool isAttacking = false;
    public LayerMask enemyMask;

    public GameObject[] enemyParts;

    


    private GameObject grid;
    void Start() {
        
            //Instanciar os objetos e os filhos (Colliders e Marcadores)
            slash1 = SlashGO.transform.GetChild(0).gameObject; //baixo
            slash2 = SlashGO.transform.GetChild(1).gameObject; //esquerda
            slash3 = SlashGO.transform.GetChild(2).gameObject; //cima
            slash4 = SlashGO.transform.GetChild(3).gameObject; //direita
            grid = SlashGO.transform.GetChild(4).gameObject;
            animator1 = slash1.GetComponent<Animator>();
            animator2 = slash2.GetComponent<Animator>();
            animator3 = slash3.GetComponent<Animator>();
            animator4 = slash4.GetComponent<Animator>();
            layerMask = LayerMask.GetMask("Skill");
        enemyParts[0] = null;
        enemyParts[1] = null;

    }

    void Update() {

        if (isAttacking)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                hideRange(); 
                this.GetComponent<battleWalk>().setSkillCommandCanvas(true);
            }
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D raycast = Physics2D.Raycast(mousePos, Vector3.forward, Mathf.Infinity, layerMask);
           
            if(raycast.collider != null)
            {
                if(raycast.collider.gameObject.GetComponent<Animator>() != null)
                {
                    raycast.collider.gameObject.GetComponent<Animator>().SetBool("slashOver", true);
                    if (Input.GetButtonDown("Fire1"))
                    {
                        checkContact(raycast.collider.name);
                    }
                }
            }
            else
            {
                animator1.SetBool("slashOver", false);
                animator2.SetBool("slashOver", false);
                animator3.SetBool("slashOver", false);
                animator4.SetBool("slashOver", false);

            }
        }



    }

    public void Attack() { //show range
        //Skill ativa
        slash1.SetActive(true);
        slash2.SetActive(true);
        slash3.SetActive(true);
        slash4.SetActive(true);
        grid.SetActive(true);
        isAttacking = true;
    }


    void checkContact(string name)
    {
        GameObject cl = GameObject.Find(name);
        RaycastHit2D line = Physics2D.Linecast(cl.transform.GetChild(0).transform.position, cl.transform.GetChild(1).transform.position
            , enemyMask);
        Debug.DrawLine(gameObject.transform.position, cl.transform.GetChild(0).transform.position, Color.blue);

        var enemy = line.collider.gameObject;




        for(int i =0; i<enemyParts.Length; i++)
        {
            if (enemyParts[i] != null)
            {
                for(int u = 0; u<enemyParts.Length; u++)
                {
                    if (enemyParts[u] == enemy)
                        break;
                }
            }
            else
                enemyParts[i] = enemy;
        }

        if (line.collider.GetComponent<Unit>().currentHP <= 0)
        {

            var en = line.collider.gameObject.transform.parent.gameObject;

            en.GetComponent<Unit>().TakeDamage(skillDamage, gameObject.GetComponent<Unit>().element);
        }
        else if(line.collider.GetComponent<Unit>().currentHP > 0)
        {
            line.collider.gameObject.GetComponent<Unit>().TakeDamage(skillDamage, gameObject.GetComponent<Unit>().element);
        }
        else
        {
            Debug.Log("NOTHING");
        }

        hideRange();
        GameObject.Find("BattleSystem").gameObject.GetComponent<battleSystem>().EndOfTurn(0);
    }

    
    


    
    
    
    void hideRange()
    {
        
        slash1.SetActive(false);
        slash2.SetActive(false);
        slash3.SetActive(false);
        slash4.SetActive(false);
        isAttacking = false;
        animator1.SetBool("isOver", false);
        animator2.SetBool("isOver", false);
        animator3.SetBool("isOver", false);
        animator4.SetBool("isOver", false);
    }
    



}
