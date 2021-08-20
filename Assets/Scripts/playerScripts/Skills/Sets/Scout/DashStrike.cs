using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashStrike:Skill
{
    private GameObject baixo;
    private GameObject cima;
    private GameObject esquerda;
    private GameObject direita;

    GameObject grid;

    private Animator animbaixo;
    private Animator animcima;
    private Animator animesquerda;
    private Animator animdireita;

    public LayerMask layermask;
    public LayerMask enemyMask;

    public GameObject testgo;

    private string sideToSend;
    [SerializeField] bool usingSkill = false;
    public DashStrike(){


    
    }


    private void Start()
    {
        baixo = this.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;
        cima = this.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.transform.GetChild(2).gameObject;
        esquerda = this.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject;
        direita = this.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.transform.GetChild(3).gameObject;

        grid = this.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.transform.GetChild(3).gameObject;

        animbaixo = baixo.GetComponent<Animator>();
        animcima = cima.GetComponent<Animator>();
        animesquerda = esquerda.GetComponent<Animator>();
        animdireita = direita.GetComponent<Animator>();
    }
    public void Attack(){
        baixo.SetActive(true);
        cima.SetActive(true);
        esquerda.SetActive(true);
        direita.SetActive(true);
        grid.SetActive(true);
        usingSkill = true;
    }

    private void Update()
    {

        if(usingSkill)
        {
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetButtonDown("Fire2"))
            {
                
                this.GetComponent<battleWalk>().setSkillCommandCanvas(true);
                hideRange();
            }

            RaycastHit2D raycast = Physics2D.Raycast(worldMousePosition, Vector3.forward, Mathf.Infinity, layermask);

            if (raycast.collider != null)
            {
                if (raycast.collider.gameObject.GetComponent<Animator>() != null)
                {
                    raycast.collider.gameObject.GetComponent<Animator>().SetBool("slashOver", true);
                    if (Input.GetButtonDown("Fire1"))
                    {
                        string checkName = raycast.collider.name;
                        sideBack(raycast.collider.name);
                        checkContact(checkName);
                    }
                }
            }
            else
            {
                animbaixo.SetBool("slashOver", false);
                animcima.SetBool("slashOver", false);
                animdireita.SetBool("slashOver", false);
                animesquerda.SetBool("slashOver", false);
            }
            /*
            if (raycast.collider.CompareTag("Skill"))
            {
                raycast.collider.gameObject.GetComponent<Animator>().SetBool("slashOver", true);
                string checkName = raycast.collider.name;
                //checkRange(checkName);

                if (Input.GetButtonDown("Fire1"))
                {
                    Debug.Log(raycast.collider.name);
                    sideBack(raycast.collider.name);
                    checkContact(checkName);

                }
            }
        

            if (raycast.collider.CompareTag("Grid"))
            {

                animbaixo.SetBool("slashOver", false);
                animcima.SetBool("slashOver", false);
                animdireita.SetBool("slashOver", false);
                animesquerda.SetBool("slashOver", false);
            }
           
            
           */
        }
    }

    void checkRange(string checkName)
    {
        GameObject CK = GameObject.Find(checkName);
        Debug.DrawLine(this.gameObject.transform.position, CK.transform.GetChild(0).transform.position, Color.blue);
       

        //Debug.DrawRay(transform.position, Vector3.forward, Color.blue, 10f);


    }

    void checkContact(string checkName)
    {
        GameObject CK = GameObject.Find(checkName);

        RaycastHit2D line = Physics2D.Linecast(transform.position, CK.transform.GetChild(0).transform.position, enemyMask);
        //Debug.Log(line.collider.name);
        if (line.collider.CompareTag("EnemyPart"))
        {
            if (line.collider.GetComponent<Unit>().currentHP <= 0)
            {
                var en = line.collider.gameObject.transform.parent.gameObject;

                en.GetComponent<Unit>().TakeDamage(skillDamage, GameObject.Find("player").gameObject.GetComponent<Unit>().element);
            }
            else
            {
                string goName = line.collider.gameObject.name;
                GameObject.Find(goName).GetComponent<Unit>().TakeDamage(skillDamage, gameObject.GetComponent<Unit>().element);
            }
            hideRange();
            movePlayer(line);
            GameObject.Find("BattleSystem").gameObject.GetComponent<battleSystem>().EndOfTurn(0);

        }
        
        
    }

    void hideRange()
    {
        baixo.SetActive(false);
        cima.SetActive(false);
        esquerda.SetActive(false);
        direita.SetActive(false);
        grid.SetActive(false);
        usingSkill = false;
        
        animbaixo.SetBool("isOver", false);
        animesquerda.SetBool("isOver", false);
        animcima.SetBool("isOver", false);
        animdireita.SetBool("isOver", false);
    }

    void movePlayer(RaycastHit2D raycast)
    {
        Debug.Log("MOVE PLAYER: " + raycast.collider.transform.position);
        
        gameObject.GetComponent<battleWalk>().dashStrikeMove(raycast.collider.transform.position, this.gameObject, sideToSend);
    }
    
    void sideBack(string sidename)
    {
        sideToSend = sidename;
    }
}
